using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;
using UnityEngine.Profiling;

namespace Unity.Entities.Editor
{

    public delegate void SystemSelectionCallback(ScriptBehaviourManager manager, World world);

    public class SystemListView : TreeView
    {
        private class AverageRecorder
        {
            private readonly Recorder recorder;
            private int frameCount;
            private int totalNanoseconds;
            private float lastReading;

            public AverageRecorder(Recorder recorder)
            {
                this.recorder = recorder;
            }

            public void Update()
            {
                ++frameCount;
                totalNanoseconds += (int)recorder.elapsedNanoseconds;
            }

            public float ReadMilliseconds()
            {
                if (frameCount > 0)
                {
                    lastReading = (totalNanoseconds/1e6f) / frameCount;
                    frameCount = totalNanoseconds = 0;
                }

                return lastReading;
            }
        }
        internal readonly Dictionary<int, ScriptBehaviourManager> managersById = new Dictionary<int, ScriptBehaviourManager>();
        private readonly Dictionary<int, World> worldsById = new Dictionary<int, World>();
        private readonly Dictionary<ScriptBehaviourManager, AverageRecorder> recordersByManager = new Dictionary<ScriptBehaviourManager, AverageRecorder>();
        private readonly Dictionary<int, HideNode> hideNodesById = new Dictionary<int, HideNode>();

        private const float kToggleWidth = 22f;
        private const float kTimingWidth = 70f;

        private readonly SystemSelectionCallback systemSelectionCallback;
        private readonly WorldSelectionGetter getWorldSelection;
        private readonly ShowInactiveSystemsGetter getShowInactiveSystems;

        private static GUIStyle RightAlignedLabel
        {
            get
            {
                if (rightAlignedText == null)
                {
                    rightAlignedText = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleRight
                    };
                }

                return rightAlignedText;
            }
        }

        private static GUIStyle rightAlignedText;

        internal static MultiColumnHeaderState GetHeaderState()
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = GUIContent.none,
                    contextMenuText = "Enabled",
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    width = kToggleWidth,
                    minWidth = kToggleWidth,
                    maxWidth = kToggleWidth,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("System Name"),
                    headerTextAlignment = TextAlignment.Left,
                    sortingArrowAlignment = TextAlignment.Right,
                    canSort = true,
                    sortedAscending = true,
                    width = 100,
                    minWidth = 100,
                    maxWidth = 2000,
                    autoResize = true,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("main (ms)"),
                    headerTextAlignment = TextAlignment.Right,
                    canSort = false,
                    width = kTimingWidth,
                    minWidth = kTimingWidth,
                    maxWidth = kTimingWidth,
                    autoResize = false,
                    allowToggleVisibility = false
                }
            };

            return new MultiColumnHeaderState(columns);
        }

        private static TreeViewState GetStateForWorld(World world, List<TreeViewState> states, List<string> stateNames)
        {
            if (world == null)
                return new TreeViewState();

            var currentWorldName = world.Name;

            var stateForCurrentWorld = states.Where((t, i) => stateNames[i] == currentWorldName).FirstOrDefault();
            if (stateForCurrentWorld != null)
                return stateForCurrentWorld;

            stateForCurrentWorld = new TreeViewState();
            states.Add(stateForCurrentWorld);
            stateNames.Add(currentWorldName);
            return stateForCurrentWorld;
        }

        public static SystemListView CreateList(List<TreeViewState> states, List<string> stateNames, SystemSelectionCallback systemSelectionCallback, WorldSelectionGetter worldSelectionGetter, ShowInactiveSystemsGetter showInactiveSystemsGetter)
        {
            var state = GetStateForWorld(worldSelectionGetter(), states, stateNames);
            var header = new MultiColumnHeader(GetHeaderState());
            return new SystemListView(state, header, systemSelectionCallback, worldSelectionGetter, showInactiveSystemsGetter);
        }

        internal SystemListView(TreeViewState state, MultiColumnHeader header, SystemSelectionCallback systemSelectionCallback, WorldSelectionGetter worldSelectionGetter, ShowInactiveSystemsGetter showInactiveSystemsGetter) : base(state, header)
        {
            this.getWorldSelection = worldSelectionGetter;
            this.systemSelectionCallback = systemSelectionCallback;
            this.getShowInactiveSystems = showInactiveSystemsGetter;
            columnIndexForTreeFoldouts = 1;
            RebuildNodes();
        }

        private HideNode CreateManagerItem(int id, ScriptBehaviourManager manager, World world)
        {
            managersById.Add(id, manager);
            worldsById.Add(id, world);
            var recorder = Recorder.Get($"{world.Name} {manager.GetType().FullName}");
            recordersByManager.Add(manager, new AverageRecorder(recorder));
            recorder.enabled = true;
            var name = getWorldSelection() == null ? $"{manager.GetType().Name} ({world.Name})" : manager.GetType().Name;
            var item = new TreeViewItem { id = id, displayName = name };
            
            var componentSystemBase = manager as ComponentSystemBase;
            var hideNode = new HideNode(item) { Active = componentSystemBase == null };
            hideNodesById.Add(id, hideNode);
            return hideNode;
        }

        private PlayerLoopSystem lastPlayerLoop;

        class HideNode
        {
            public readonly TreeViewItem Item;
            public bool Active = true;
            public List<HideNode> Children;

            public HideNode(TreeViewItem item)
            {
                Item = item;
            }

            public void AddChild(HideNode child)
            {
                if (Children == null)
                    Children = new List<HideNode>();
                Children.Add(child);
            }

            public TreeViewItem BuildList(bool showInactiveSystems)
            {
                if (showInactiveSystems || Active)
                {
                    Item.children = null;
                    if (Children != null)
                    {
                        Item.children = new List<TreeViewItem>();
                        foreach (var child in Children)
                        {
                            var childItem = child.BuildList(showInactiveSystems);
                            if (childItem != null)
                                Item.children.Add(childItem);
                        }
                    }
                    return Item;
                    
                }
                else
                {
                    return null;
                }
            }
        }

        private HideNode rootNode;

        private void RebuildNodes()
        {
            rootNode = null;
            Reload();
        }

        private List<int> BuildNodeTree()
        {
            managersById.Clear();
            worldsById.Clear();
            recordersByManager.Clear();
            hideNodesById.Clear();

            var currentID = 0;
            rootNode = new HideNode(new TreeViewItem {id = currentID++, depth = -1, displayName = "Root"});

            lastPlayerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;

            var expandedIds = new List<int>();

            if (ScriptBehaviourUpdateOrder.CurrentPlayerLoop.subSystemList != null)
            {
                foreach (var group in ScriptBehaviourUpdateOrder.CurrentPlayerLoop.subSystemList)
                {
                    var groupItem = new HideNode(new TreeViewItem
                        {id = currentID++, depth = 0, displayName = group.type.Name});
                    var hasManagerChildren = false;
                    foreach (var child in group.subSystemList)
                    {
                        var executionDelegate = child.updateDelegate;
                        ScriptBehaviourUpdateOrder.DummyDelagateWrapper dummy;
                        if (executionDelegate != null &&
                            (dummy = executionDelegate.Target as ScriptBehaviourUpdateOrder.DummyDelagateWrapper) != null)
                        {
                            var system = dummy.Manager;
                            if (getWorldSelection() == null)
                            {
                                foreach (var world in World.AllWorlds)
                                {
                                    if (world.BehaviourManagers.Contains(system))
                                    {
                                        groupItem.AddChild(CreateManagerItem(currentID++, system, world));
                                        hasManagerChildren = true;
                                    }
                                }
                            }
                            else
                            {
                                if (getWorldSelection().BehaviourManagers.Contains(system))
                                {
                                    groupItem.AddChild(CreateManagerItem(currentID++, system, getWorldSelection()));
                                    hasManagerChildren = true;
                                }
                            }
                        }
                        else if (getWorldSelection() == null)
                        {
                            groupItem.AddChild(new HideNode(new TreeViewItem(currentID++, 1, child.type.Name)));
                        }
                    }
    
                    if (groupItem.Children != null)
                    {
                        rootNode.AddChild(groupItem);
                        if (hasManagerChildren)
                            expandedIds.Add(groupItem.Item.id);
                    }
                    else
                    {
                        --currentID;
                    }
                }
            }

            return expandedIds;
        }

        protected override TreeViewItem BuildRoot()
        {
            if (rootNode == null)
                state.expandedIDs = BuildNodeTree();
            
            var root = rootNode.BuildList(getShowInactiveSystems());
            
            if (!root.hasChildren)
                root.children = new List<TreeViewItem>(0);
            
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void BeforeRowsGUI()
        {
            var becameVisible = false;
            foreach (var idManagerPair in managersById)
            {
                var componentSystemBase = idManagerPair.Value as ComponentSystemBase;
                if (componentSystemBase == null)
                    continue;
                var hideNode = hideNodesById[idManagerPair.Key];
                if (componentSystemBase.ShouldRunSystem() && !hideNode.Active)
                {
                    hideNode.Active = true;
                    becameVisible = true;
                }
            }
            if (becameVisible)
                Reload();
            base.BeforeRowsGUI();
        }

        protected override void RowGUI (RowGUIArgs args)
        {
            if (args.item.depth == -1)
                return;
            var item = args.item;

            var enabled = GUI.enabled;

            if (managersById.ContainsKey(item.id))
            {
                var manager = managersById[item.id];
                var componentSystemBase = manager as ComponentSystemBase;
                if (componentSystemBase != null)
                {
                    var toggleRect = args.GetCellRect(0);
                    toggleRect.xMin = toggleRect.xMin + 4f;
                    componentSystemBase.Enabled = GUI.Toggle(toggleRect, componentSystemBase.Enabled, GUIContent.none);
                }

                if (componentSystemBase != null)
                {
                    var timingRect = args.GetCellRect(2);
                    if (componentSystemBase.ShouldRunSystem())
                    {
                        var recorder = recordersByManager[manager];
                        GUI.Label(timingRect, recorder.ReadMilliseconds().ToString("f2"), RightAlignedLabel);
                    }
                    else
                    {
                        GUI.enabled = false;
                        GUI.Label(timingRect, "not run", RightAlignedLabel);
                        GUI.enabled = enabled;
                    }
                }
            }
            else
            {
                GUI.enabled = false;
            }

            var indent = GetContentIndent(item);
            var nameRect = args.GetCellRect(1);
            nameRect.xMin = nameRect.xMin + indent;
            GUI.Label(nameRect, item.displayName);
            GUI.enabled = enabled;
        }

        protected override void AfterRowsGUI()
        {
            base.AfterRowsGUI();
            if (Event.current.type == EventType.MouseDown)
            {
                SetSelection(new List<int>());
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0 && managersById.ContainsKey(selectedIds[0]))
            {
                systemSelectionCallback(managersById[selectedIds[0]], worldsById[selectedIds[0]]);
            }
            else
            {
                systemSelectionCallback(null, null);
                SetSelection(new List<int>());
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        public void TouchSelection()
        {
            SetSelection(GetSelection(), TreeViewSelectionOptions.FireSelectionChanged);
        }

        private bool PlayerLoopsMatch(PlayerLoopSystem a, PlayerLoopSystem b)
        {
            if (a.type != b.type)
                return false;
            if (a.subSystemList == b.subSystemList)
                return true;
            if (a.subSystemList == null || b.subSystemList == null)
                return false;
            if (a.subSystemList.Length != b.subSystemList.Length)
                return false;
            for (var i = 0; i < a.subSystemList.Length; ++i)
            {
                if (!PlayerLoopsMatch(a.subSystemList[i], b.subSystemList[i]))
                    return false;
            }
            
            return true;
        }

        public bool NeedsReload => !PlayerLoopsMatch(lastPlayerLoop, ScriptBehaviourUpdateOrder.CurrentPlayerLoop);

        public void ReloadIfNecessary()
        {
            if (NeedsReload)
                RebuildNodes();
        }

        private int lastTimedFrame;

        public void UpdateTimings()
        {
            if (Time.frameCount == lastTimedFrame)
                return;

            foreach (var recorder in recordersByManager.Values)
            {
                recorder.Update();
            }

            lastTimedFrame = Time.frameCount;
        }

        public void SetSystemSelection(ScriptBehaviourManager manager, World world)
        {
            foreach (var pair in managersById)
            {
                if (pair.Value == manager)
                {
                    SetSelection(new List<int> {pair.Key});
                    return;
                }
            }
            SetSelection(new List<int>());
        }
    }
}
