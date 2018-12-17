using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities.Editor
{
    
    public delegate void SetEntityListSelection(EntityListQuery query);
    
    public class ComponentGroupListView : TreeView {
        private static Dictionary<ComponentSystemBase, List<EntityArchetypeQuery>> queriesBySystem = new Dictionary<ComponentSystemBase, List<EntityArchetypeQuery>>();
        private static readonly Dictionary<ComponentGroup, EntityArchetypeQuery> queriesByGroup = new Dictionary<ComponentGroup, EntityArchetypeQuery>();
        
        private static EntityArchetypeQuery GetQueryForGroup(ComponentGroup group)
        {
            if (!queriesByGroup.ContainsKey(group))
            {
                var query = new EntityArchetypeQuery()
                {
                    All = group.GetQueryTypes().Where(x => x.AccessModeType != ComponentType.AccessMode.Subtractive).ToArray(),
                    Any = new ComponentType[0],
                    None = group.GetQueryTypes().Where(x => x.AccessModeType == ComponentType.AccessMode.Subtractive).ToArray()
                };
                queriesByGroup.Add(group, query);
            }

            return queriesByGroup[group];
        }
        
        private readonly Dictionary<int, ComponentGroup> componentGroupsById = new Dictionary<int, ComponentGroup>();
        private readonly Dictionary<int, EntityArchetypeQuery> queriesById = new Dictionary<int, EntityArchetypeQuery>();
        private readonly Dictionary<int, ComponentGroupGUIControl> controlsById = new Dictionary<int, ComponentGroupGUIControl>();

        public ComponentSystemBase SelectedSystem
        {
            get { return selectedSystem; }
            set
            {
                if (selectedSystem != value)
                {
                    selectedSystem = value;
                    Reload();
                }
            }
        }
        private ComponentSystemBase selectedSystem;

        private readonly WorldSelectionGetter getWorldSelection;
        private readonly SetEntityListSelection entityListSelectionCallback;

        private static TreeViewState GetStateForSystem(ComponentSystemBase system, List<TreeViewState> states, List<string> stateNames)
        {
            if (system == null)
                return new TreeViewState();
            
            var currentSystemName = system.GetType().FullName;

            var stateForCurrentSystem = states.Where((t, i) => stateNames[i] == currentSystemName).FirstOrDefault();
            if (stateForCurrentSystem != null)
                return stateForCurrentSystem;
            
            stateForCurrentSystem = new TreeViewState();
            if (system.ComponentGroups != null && system.ComponentGroups.Length > 0)
                stateForCurrentSystem.expandedIDs = new List<int> {1};
            states.Add(stateForCurrentSystem);
            stateNames.Add(currentSystemName);
            return stateForCurrentSystem;
        }

        public static ComponentGroupListView CreateList(ComponentSystemBase system, List<TreeViewState> states, List<string> stateNames,
            SetEntityListSelection entityQuerySelectionCallback, WorldSelectionGetter worldSelectionGetter)
        {
            var state = GetStateForSystem(system, states, stateNames);
            return new ComponentGroupListView(state, system, entityQuerySelectionCallback, worldSelectionGetter);
        }

        public ComponentGroupListView(TreeViewState state, ComponentSystemBase system, SetEntityListSelection entityListSelectionCallback, WorldSelectionGetter worldSelectionGetter) : base(state)
        {
            this.getWorldSelection = worldSelectionGetter;
            this.entityListSelectionCallback = entityListSelectionCallback;
            selectedSystem = system;
            rowHeight += 1;
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        public float Height { get; private set; }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            return controlsById.ContainsKey(item.id) ? controlsById[item.id].Height + 2 : rowHeight;
        }

        private static List<EntityArchetypeQuery> GetQueriesForSystem(ComponentSystemBase system)
        {
            List<EntityArchetypeQuery> queries;
            if (queriesBySystem.TryGetValue(system, out queries))
                return queries;
            
            queries = new List<EntityArchetypeQuery>();

            var currentType = system.GetType();

            while (currentType != null)
            {
                foreach (var field in currentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (field.FieldType == typeof(EntityArchetypeQuery))
                        queries.Add(field.GetValue(system) as EntityArchetypeQuery);
                }

                currentType = currentType.BaseType;
            }

            return queries;
        }

        protected override TreeViewItem BuildRoot()
        {
            componentGroupsById.Clear();
            queriesById.Clear();
            controlsById.Clear();
            var currentId = 0;
            var root  = new TreeViewItem { id = currentId++, depth = -1, displayName = "Root" };
            if (getWorldSelection() == null)
            {
                root.AddChild(new TreeViewItem { id = currentId, displayName = "No world selected"});
            }
            else if (SelectedSystem == null)
            {
                root.AddChild(new TreeViewItem { id = currentId, displayName = "Null System"});
            }
            else
            {
                var queries = GetQueriesForSystem(SelectedSystem);

                foreach (var query in queries)
                {
                    queriesById.Add(currentId, query);
                    var queryItem = new TreeViewItem { id = currentId++ };
                    root.AddChild(queryItem);
                }
                if (SelectedSystem.ComponentGroups != null)
                {
                    foreach (var group in SelectedSystem.ComponentGroups)
                    {
                        componentGroupsById.Add(currentId, group);

                        var groupItem = new TreeViewItem { id = currentId++ };
                        root.AddChild(groupItem);
                    }
                }
                if (queriesById.Count == 0 && componentGroupsById.Count == 0)
                {
                    root.AddChild(new TreeViewItem { id = currentId, displayName = "No Component Groups or Queries in Manager"});
                }
                else
                {
                    SetupDepthsFromParentsAndChildren(root);
                    
                    foreach (var idGroupPair in componentGroupsById)
                    {
                        var newControl = new ComponentGroupGUIControl(idGroupPair.Value.GetQueryTypes(), idGroupPair.Value.GetReadAndWriteTypes(), true);
                        controlsById.Add(idGroupPair.Key, newControl);
                    }
                    foreach (var idQueryPair in queriesById)
                    {
                        var types = idQueryPair.Value.All.Concat(idQueryPair.Value.Any);
                        types = types.Concat(idQueryPair.Value.None.Select(x => ComponentType.Subtractive(x.GetManagedType())));
                
                        var newControl = new ComponentGroupGUIControl(types, true);
                        controlsById.Add(idQueryPair.Key, newControl);
                    }
                }
            }
            return root;
        }

        private float width;
        private const float kBorderWidth = 60f;

        public void SetWidth(float newWidth)
        {
            newWidth -= kBorderWidth;
            if (newWidth != width)
            {
                width = newWidth;
                foreach (var control in controlsById.Values)
                    control.UpdateSize(width);
            }
            RefreshCustomRowHeights();
            var height = 0f;
            foreach (var child in rootItem.children)
                height += GetCustomRowHeight(0, child);
            Height = height;
        }

        public override void OnGUI(Rect rect)
        {
            if (getWorldSelection()?.GetExistingManager<EntityManager>()?.IsCreated == true)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    SetWidth(rect.width);
                }
                base.OnGUI(rect);
            }
        }

        protected void DrawCount(RowGUIArgs args)
        {
            ComponentGroup componentGroup;
            if (componentGroupsById.TryGetValue(args.item.id, out componentGroup))
            {
                var countString = componentGroup.CalculateLength().ToString();
                DefaultGUI.LabelRightAligned(args.rowRect, countString, args.selected, args.focused);
            }
            else
            {
                EntityArchetypeQuery query;
                if (queriesById.TryGetValue(args.item.id, out query))
                {
                    var entityManager = getWorldSelection().GetExistingManager<EntityManager>();
                    var chunkArray = entityManager.CreateArchetypeChunkArray(query, Allocator.TempJob);
                    var count = chunkArray.Sum(x => x.Count);
                    chunkArray.Dispose();
                    DefaultGUI.LabelRightAligned(args.rowRect, count.ToString(), args.selected, args.focused);
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
            if (Event.current.type != EventType.Repaint || !controlsById.ContainsKey(args.item.id))
                return;

            var position = args.rowRect.position;
            position.x = GetContentIndent(args.item);
            position.y += 1;

            controlsById[args.item.id].OnGUI(position);

            DrawCount(args);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0)
            {
                ComponentGroup componentGroup;
                if (componentGroupsById.TryGetValue(selectedIds[0], out componentGroup))
                    entityListSelectionCallback(new EntityListQuery(componentGroup));
                else
                {
                    EntityArchetypeQuery query;
                    if (queriesById.TryGetValue(selectedIds[0], out query))
                        entityListSelectionCallback(new EntityListQuery(query));
                }
            }
            else
            {
                entityListSelectionCallback(null);
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        public void SetEntityListSelection(EntityListQuery newListQuery)
        {
            if (newListQuery == null)
            {
                SetSelection(new List<int>());
                return;
            }
            if (newListQuery.Group != null)
            {
                foreach (var pair in componentGroupsById)
                {
                    if (pair.Value == newListQuery.Group)
                    {
                        SetSelection(new List<int> {pair.Key});
                        return;
                    }
                }
            }
            else
            {
                foreach (var pair in queriesById)
                {
                    if (pair.Value == newListQuery.Query)
                    {
                        SetSelection(new List<int> {pair.Key});
                        return;
                    }
                }
            }
            SetSelection(new List<int>());
        }

        public void SetComponentGroupSelection(ComponentGroup group)
        {
            SetSelection(new List<int>());
        }

        public void TouchSelection()
        {
            SetSelection(GetSelection(), TreeViewSelectionOptions.FireSelectionChanged);
        }
        
        public bool NeedsReload
        {
            get
            {
                var expectedGroupCount = SelectedSystem?.ComponentGroups?.Length ?? 0;
                return expectedGroupCount != componentGroupsById.Count;
            }
            
        }

        public void ReloadIfNecessary()
        {
            if (NeedsReload)
                Reload();
        }
    }
}
