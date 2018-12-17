using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Unity.Entities.Editor
{
    
    public delegate void SetChunkFilter(ChunkFilter filter);

    public class ChunkFilter
    {
        public int firstIndex;
        public int lastIndex;
    }
    
    public class ChunkInfoListView : TreeView, IDisposable
    {

        private static readonly float kHistogramHeight = 60f;
        
        [System.Serializable]
        public class State
        {
            public TreeViewState state = new TreeViewState();
            public ViewMode mode = ViewMode.Stats;
        }

        private State listState;

        private static Material HistogramMaterial
        {
            get
            {
                if (!histogramMaterial)
                {
                    histogramMaterial = new Material(Shader.Find("Unlit/Color")) {color = new Color(1f, 0.6f, 0f)};
                    histogramMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return histogramMaterial;
            }
        }

        private static Material histogramMaterial;
        
        public enum ViewMode
        {
            Chunks,
            Stats
        }

        private class ArchetypeInfo : IDisposable
        {
            internal static class Styles
            {
                public static readonly GUIStyle labelStyleUpperLeft;
                public static readonly GUIStyle labelStyleUpperRight;
                public static readonly GUIStyle labelStyleLowerLeft;
                public static readonly GUIStyle labelStyleLowerRight;
                public static readonly GUIStyle labelStyleUpperCenter;
                public static readonly GUIContent reusableLabel = new GUIContent();
                public static readonly GUIContent zeroLabel = new GUIContent("0");
                public static readonly GUIContent oneLabel = new GUIContent("1");
                public static readonly GUIContent xAxisLabel;

                static Styles()
                {
                    labelStyleUpperLeft = new GUIStyle(EditorStyles.miniLabel) {alignment = TextAnchor.UpperLeft};
                    labelStyleUpperRight = new GUIStyle(EditorStyles.miniLabel) {alignment = TextAnchor.UpperRight};
                    labelStyleLowerLeft = new GUIStyle(EditorStyles.miniLabel) {alignment = TextAnchor.LowerLeft};
                    labelStyleLowerRight = new GUIStyle(EditorStyles.miniLabel) {alignment = TextAnchor.LowerRight};
                    labelStyleUpperCenter = new GUIStyle(EditorStyles.miniLabel) {alignment = TextAnchor.UpperCenter};
                    xAxisLabel = new GUIContent(L10n.Tr("Chunk Utilization"));
                }
            }
            
            public EntityArchetype archetype;
            public ComponentGroupGUIControl control;
            public int[] counts;
            public int maxCount;
            public int firstChunkIndex;
            public int lastChunkIndex;
            public Mesh histogramMesh;

            public void IncrementCount(int index)
            {
                --index;
                ++counts[index];
                if (maxCount < counts[index])
                    maxCount = counts[index];
            }

            public void UpdateHistogram()
            {
                var vertices = new Vector3[counts.Length*4];
                var triangles = new int[counts.Length * 6];
                var xIncrement = 1f / (counts.Length + 1);
                var yIncrement = 1f / maxCount;
                var minHeight = Mathf.Max(yIncrement, 0.03f);
                var barWidth = Mathf.Max(xIncrement, 0.01f);
                for (var i = 0; i < counts.Length; ++i)
                {
                    var firstVertexIndex = i * 4;
                    var barHeight = counts[i] * yIncrement;
                    if (barHeight > 0f && barHeight < minHeight)
                        barHeight = minHeight;
                    vertices[firstVertexIndex + 0] = new Vector3(i*xIncrement, 1f - barHeight, 0f);
                    vertices[firstVertexIndex + 1] = new Vector3(i*xIncrement, 1f, 0f);
                    vertices[firstVertexIndex + 2] = new Vector3(i*xIncrement + barWidth, 1f, 0f);
                    vertices[firstVertexIndex + 3] = new Vector3(i*xIncrement + barWidth, 1f - barHeight, 0f);
                    
                    var firstTriangleIndex = i * 6;
                    triangles[firstTriangleIndex + 0] = firstVertexIndex + 0;
                    triangles[firstTriangleIndex + 1] = firstVertexIndex + 2;
                    triangles[firstTriangleIndex + 2] = firstVertexIndex + 1;
                    triangles[firstTriangleIndex + 3] = firstVertexIndex + 2;
                    triangles[firstTriangleIndex + 4] = firstVertexIndex + 0;
                    triangles[firstTriangleIndex + 5] = firstVertexIndex + 3;
                }
                histogramMesh = new Mesh();
                histogramMesh.vertices = vertices;
                histogramMesh.triangles = triangles;
                histogramMesh.hideFlags = HideFlags.HideAndDontSave;

                maxCountString = maxCount.ToString();
                countsLengthString = counts.Length.ToString();
            }

            private float kMinMargin = 15f;
            private string maxCountString;
            private string countsLengthString;

            public void DrawHistogram(Rect rect)
            {
                Styles.reusableLabel.text = maxCountString;
                var xMargin = Mathf.Max(Styles.labelStyleUpperRight.CalcSize(Styles.reusableLabel).x, kMinMargin);
                
                GUI.Label(new Rect(rect.x, rect.y, xMargin, kMinMargin), Styles.reusableLabel, Styles.labelStyleUpperRight);
                GUI.Label(new Rect(rect.x, rect.yMax - 2*kMinMargin, xMargin, kMinMargin), Styles.zeroLabel, Styles.labelStyleLowerRight);
                
                Styles.reusableLabel.text = maxCountString;
                GUI.Label(new Rect(rect.xMax - xMargin, rect.yMin, xMargin, kMinMargin), Styles.reusableLabel, Styles.labelStyleUpperLeft);
                GUI.Label(new Rect(rect.xMax - xMargin, rect.yMax - 2*kMinMargin, xMargin, kMinMargin), Styles.zeroLabel, Styles.labelStyleLowerLeft);
                
                GUI.Label(new Rect(rect.x + xMargin, rect.yMax - kMinMargin, kMinMargin, kMinMargin), Styles.oneLabel, Styles.labelStyleUpperLeft);
                Styles.reusableLabel.text = countsLengthString;
                GUI.Label(new Rect(rect.xMax - xMargin - 3*kMinMargin, rect.yMax - kMinMargin, 3*kMinMargin, kMinMargin), Styles.reusableLabel, Styles.labelStyleUpperRight);

                GUI.Label(new Rect(rect.x + kMinMargin, rect.yMax - kMinMargin, rect.width - 2*kMinMargin, kMinMargin), Styles.xAxisLabel, Styles.labelStyleUpperCenter);

                rect.xMin = rect.xMin + xMargin;
                rect.xMax = rect.xMax - xMargin;
                rect.yMax = rect.yMax - kMinMargin;
                var translation = new Vector3(rect.x, rect.y, 0f);
                var scale = new Vector3(rect.width, rect.height, 1f);
                var matrix = GUI.matrix *
                             Matrix4x4.Translate(translation) *
                             Matrix4x4.Scale(scale);
                HistogramMaterial.SetPass(0);
                Graphics.DrawMeshNow(histogramMesh, matrix);
            }

            public void Dispose()
            {
                if (histogramMesh)
                    UnityEngine.Object.DestroyImmediate(histogramMesh);
            }
        }

        public ViewMode Mode {
            get { return listState.mode; }
            set
            {
                if (listState.mode != value)
                {
                    listState.mode = value;
                    Reload();
                }
            }
        }

        private Dictionary<int, ArchetypeInfo> archetypeInfoById = new Dictionary<int, ArchetypeInfo>();

        private SetChunkFilter setChunkFilter;
        
        private NativeArray<ArchetypeChunk> chunkArray;
        
        public ChunkInfoListView(State listState, SetChunkFilter setChunkFilter) : base(listState.state)
        {
            this.listState = listState;
            this.setChunkFilter = setChunkFilter;
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        public void SetChunkArray(NativeArray<ArchetypeChunk> newChunkArray)
        {
            chunkArray = newChunkArray;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            foreach (var info in archetypeInfoById.Values)
                info.Dispose();
            archetypeInfoById.Clear();
            var currentNonChunkId = -1;
            var currentChunkIndex = 0;
            var root  = new TreeViewItem { id = currentNonChunkId--, depth = -1, displayName = "Root" };
            if (!chunkArray.IsCreated)
            {
                root.children = new List<TreeViewItem>();
            }
            else
            {
                var currentArchetype = new EntityArchetype();
                TreeViewItem currentArchetypeItem = null;
                foreach (var chunk in chunkArray)
                {
                    if (chunk.Archetype != currentArchetype)
                    {
                        currentArchetype = chunk.Archetype;
                        var stats = new ArchetypeInfo()
                        {
                            archetype = currentArchetype,
                            control = new ComponentGroupGUIControl(currentArchetype.ComponentTypes, true),
                            counts = new int[currentArchetype.ChunkCapacity],
                            firstChunkIndex = currentChunkIndex
                        };
                        archetypeInfoById.Add(currentNonChunkId, stats);
                        currentArchetypeItem = new TreeViewItem() { id = currentNonChunkId--, depth = 0 };
                        root.AddChild(currentArchetypeItem);
                    }

                    archetypeInfoById[currentArchetypeItem.id].IncrementCount(chunk.Count);
                    archetypeInfoById[currentArchetypeItem.id].lastChunkIndex = currentChunkIndex;
                    
                    if (Mode == ViewMode.Chunks)
                    {
                        var chunkItem = new TreeViewItem() { id = currentChunkIndex, depth = 1, displayName = chunk.Count.ToString() };
                        currentArchetypeItem.AddChild(chunkItem);
                    }
                    ++currentChunkIndex;
                }
                if (!root.hasChildren)
                    root.children = new List<TreeViewItem>();
            }

            foreach (var info in archetypeInfoById.Values)
                info.UpdateHistogram();

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            if (Event.current.type == EventType.Repaint)
            {
                if (archetypeInfoById.ContainsKey(args.item.id))
                {
                    var position = args.rowRect.position;
                    position.x = GetContentIndent(args.item);
                    position.y += 1;
                    var info = archetypeInfoById[args.item.id];
                    info.control.OnGUI(position);
                    DefaultGUI.LabelRightAligned(args.rowRect, info.archetype.ChunkCount.ToString(), args.selected, args.focused);
                    if (listState.mode == ViewMode.Stats)
                    {
                        var histogramRect = args.rowRect;
                        histogramRect.yMin = histogramRect.yMax - kHistogramHeight;
                        info.DrawHistogram(histogramRect);
                    }
                }
            }
            base.RowGUI(args);
        }

        private float width;
        
        public override void OnGUI(Rect rect)
        {
            var newWidth = rect.width - 40f;
            if (newWidth != width)
            {
                width = newWidth;
                foreach (var info in archetypeInfoById.Values)
                    info.control.UpdateSize(width);
            }
            RefreshCustomRowHeights();
            
            base.OnGUI(rect);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0)
            {
                if (archetypeInfoById.ContainsKey(selectedIds[0]))
                {
                    var info = archetypeInfoById[selectedIds[0]];
                    setChunkFilter(new ChunkFilter() { firstIndex = info.firstChunkIndex, lastIndex = info.lastChunkIndex });
                }
                else
                {
                    setChunkFilter(new ChunkFilter() { firstIndex = selectedIds[0], lastIndex = selectedIds[0]});
                }
            }
            else
            {
                setChunkFilter(null);
            }
            
            base.SelectionChanged(selectedIds);
        }
        
        protected override void AfterRowsGUI()
        {
            base.AfterRowsGUI();
            if (Event.current.type == EventType.MouseDown)
            {
                SetSelection(new List<int>(), TreeViewSelectionOptions.FireSelectionChanged);
            }
        }

        float GetArchetypeHeight(int id)
        {
            return archetypeInfoById[id].control.Height + 2 + (listState.mode == ViewMode.Stats ? kHistogramHeight : 0);
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            return archetypeInfoById.ContainsKey(item.id) ? GetArchetypeHeight(item.id) : rowHeight;
        }

        public void ClearSelection()
        {
            SetSelection(new List<int>(), TreeViewSelectionOptions.FireSelectionChanged);
        }

        public void Dispose()
        {
            if (histogramMaterial)
                UnityEngine.Object.DestroyImmediate(histogramMaterial);
            foreach (var info in archetypeInfoById.Values)
                info.Dispose();
        }
    }
}
