namespace UntitledGames.Hierarchy
{
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
#else
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

#endif

    public class HierarchyProEditorGroupWindow : EditorWindow
    {
        private static HierarchyProEditorGroupWindow instance;
        private bool redrawPending = true;
        private Vector2 scrollPosition;
        private HierarchyProGroup renaming;
        private string renamingName;
        private HierarchyProEditorTrackDrag trackDrag;
        private IEnumerable<HierarchyProEditorGroupLine> groupData;
        private HierarchyProGroup dragging;
        private Vector2 dragPosition = new Vector2(0, -100);

        public static HierarchyProGroup Dragging { get { return HierarchyProEditorGroupWindow.Instance.dragging; } }

        public static HierarchyProEditorGroupWindow Instance { get { return HierarchyProEditorGroupWindow.instance; } }

        public static bool RedrawPending
        {
            get
            {
                if (HierarchyProEditorGroupWindow.Instance == null)
                {
                    return false;
                }
                bool result = HierarchyProEditorGroupWindow.Instance.redrawPending;
                HierarchyProEditorGroupWindow.Instance.redrawPending = false;
                return result;
            }
        }

        public static HierarchyProGroup Renaming { get { return HierarchyProEditorGroupWindow.Instance.renaming; } set { HierarchyProEditorGroupWindow.Instance.renaming = value; } }

        public static string RenamingName { get { return HierarchyProEditorGroupWindow.Instance.renamingName; } set { HierarchyProEditorGroupWindow.Instance.renamingName = value; } }

        public static HierarchyProGroup Selected
        {
            get { return HierarchyProGroupLibrary.Selected; }
            set
            {
                HierarchyProGroupLibrary.SelectedID = value == null ? 0 : value.ID;
                EditorUtility.SetDirty(HierarchyProGroupLibrary.Instance);

                HierarchyProEditorGroupWindow.Renaming = null;
                HierarchyProEditorGroupWindow.Redraw();
            }
        }

        public static float Width { get { return HierarchyProEditorGroupWindow.Instance == null ? 0 : HierarchyProEditorGroupWindow.Instance.position.width; } }

        [MenuItem("Window/Groups")]
        public static void Create()
        {
            HierarchyProEditorGroupWindow window = EditorWindow.GetWindow<HierarchyProEditorGroupWindow>();

            GUIContent titleContent = new GUIContent("Groups", HierarchyProEditorIcons.Group);
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
            window.titleContent = titleContent;
#else
            window.title = titleContent.text;

            PropertyInfo titleInfo = window.GetType().GetProperty("cachedTitleContent", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            GUIContent title = titleInfo.GetValue(window, null) as GUIContent;
            title.image = titleContent.image;

#endif
            window.autoRepaintOnSceneChange = true;

            window.Show();
        }

        public static void Redraw()
        {
            if (HierarchyProEditorGroupWindow.Instance != null)
            {
                HierarchyProEditorGroupWindow.Instance.Repaint();
            }
        }

        private static void DrawNoGroupsMessage(Rect rect)
        {
            Rect notice = new Rect(rect.x + 10, rect.y + 10, rect.width - 20, rect.height - 20);
            GUI.color = new Color(1, 1, 1, 0.3f);
            GUI.Label(new Rect(notice.x, notice.y, notice.width, notice.height / 2), "No groups", HierarchyProEditorStyles.LabelLargeCentered);
            GUI.Label(new Rect(notice.x, notice.y + (notice.height / 2), notice.width, notice.height / 2), "Select game objects and press Create.", HierarchyProEditorStyles.LabelTinyCentered);
            GUI.color = Color.white;
        }

        private static IEnumerable<HierarchyProEditorGroupLine> StackGroups(Rect rect)
        {
            Rect lineRect = new Rect(rect) {height = 16};
            List<HierarchyProEditorGroupLine> lines = new List<HierarchyProEditorGroupLine>();
            foreach (HierarchyProGroup group in HierarchyProGroupLibrary.RootGroups)
            {
                HierarchyProEditorGroupWindow.StackGroupsRecursive(ref lines, ref lineRect, group);
            }
            return lines;
        }

        private static void StackGroupsRecursive(ref List<HierarchyProEditorGroupLine> lines, ref Rect lineRect, HierarchyProGroup group, int indent = 0)
        {
            lines.Add(HierarchyProEditorGroupLine.Create(group, lineRect, indent));
            lineRect.y += lineRect.height;

            if (group.ShowChildren)
            {
                IEnumerable<HierarchyProGroup> children = HierarchyProGroupLibrary.FindChildren(group).ToList();
                group.HasChildren = children.Any();
                foreach (HierarchyProGroup child in children)
                {
                    HierarchyProEditorGroupWindow.StackGroupsRecursive(ref lines, ref lineRect, child, indent + 1);
                }
            }
        }

        private void DrawToolbar(Rect rect)
        {
            Rect add = new Rect(rect) {x = rect.x + 6, width = 52};
            if (GUI.Button(add, "Create", EditorStyles.toolbarButton))
            {
                HierarchyProGroup group = HierarchyProGroup.Create();
                group.AddObjects(Selection.objects);
                if (HierarchyProEditorGroupWindow.Selected != null)
                {
                    group.Parent = HierarchyProEditorGroupWindow.Selected;
                }
                group.GenerateName();

                HierarchyProGroupLibrary.Add(group);
                HierarchyProEditorGroupWindow.Selected = group;

                EditorUtility.SetDirty(HierarchyProGroupLibrary.Instance);
            }

            Rect settings = new Rect(rect) {x = rect.xMax - 32, width = 26};
            if (GUI.Button(settings, new GUIContent(HierarchyProEditorIcons.Cog), EditorStyles.toolbarButton))
            {
                if (Event.current.control)
                {
                    Object.DestroyImmediate(HierarchyProGroupLibrary.Instance.gameObject);
                }
                if (Event.current.shift)
                {
                    Selection.activeObject = HierarchyProGroupLibrary.Instance.gameObject;
                }

                Vector2 centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Vector2 rectPreferencesSize = new Vector2(500, 340);
                var rectPreferencesCenter = centerScreen - (rectPreferencesSize / 2f);
                Rect rectPreferences = new Rect(rectPreferencesCenter.x, rectPreferencesCenter.y, rectPreferencesSize.x, rectPreferencesSize.y);
                HierarchyProPreferences preferences = EditorWindow.GetWindowWithRect<HierarchyProPreferences>(rectPreferences, true, "HierarchyPro Preferences", true);
                preferences.Show();
            }
        }

        private void Drop(HierarchyProGroup dragGroup, Vector2 dropPosition)
        {
            foreach (HierarchyProEditorGroupLine line in this.groupData)
            {
                Rect dropRect = line.Rect;
                if (line.Group == this.dragging)
                {
                    dropRect.width = 16;
                }
                if (dropRect.Contains(dropPosition))
                {
                    if (line.Group == dragGroup)
                    {
                        // Dropped line on itself.
                        dragGroup.Parent = null;
                        return;
                    }

                    if (line.Group.IsChildOf(dragGroup))
                    {
                        line.Group.Parent = null;
                    }
                    dragGroup.Parent = line.Group;
                }
            }
        }

        private void HandleNativeDrag()
        {
            foreach (HierarchyProEditorGroupLine line in this.groupData)
            {
                if (line.Rect.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;

                    if (Event.current.type == EventType.DragPerform)
                    {
                        line.Group.AddObjects(DragAndDrop.objectReferences);
                        DragAndDrop.AcceptDrag();
                    }

                    return;
                }
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        }

        private void OnEnable()
        {
            HierarchyProEditorGroupWindow.instance = this;
            this.trackDrag = new HierarchyProEditorTrackDrag();
            this.trackDrag.Drag += this.TrackDragOnDrag;
            this.trackDrag.Drop += this.TrackDragOnDrop;
            this.trackDrag.Dragging += this.TrackDragOnDragging;

            this.wantsMouseMove = true;
        }

        private void OnGUI()
        {
            Rect window = this.position;
            Rect toolbar = new Rect(0, 0, window.width, 18);
            Rect rect = new Rect(0, toolbar.yMax, window.width, window.height - toolbar.yMax);

            GUI.Label(toolbar, GUIContent.none, HierarchyProEditorStyles.ToolbarHorizontal);
            this.DrawToolbar(toolbar);

            if (HierarchyProGroupLibrary.Count == 0)
            {
                HierarchyProEditorGroupWindow.DrawNoGroupsMessage(rect);
                return;
            }

            this.groupData = HierarchyProEditorGroupWindow.StackGroups(rect).ToList();
            float height = (this.groupData != null) && this.groupData.Any() ? this.groupData.Max(x => x.Rect.yMax) - rect.y : 0;
            Rect scrollRect = new Rect(rect) {width = rect.width - 18, height = height};
            this.scrollPosition = GUI.BeginScrollView(rect, this.scrollPosition, scrollRect);

            this.trackDrag.Update(Event.current);
            this.HandleNativeDrag();

            bool hasScrollbar = scrollRect.height > rect.height;
            foreach (HierarchyProEditorGroupLine line in this.groupData)
            {
                bool dragOver = false;
                if (this.dragging != null)
                {
                    Rect dropRect = line.Rect;
                    if (line.Group == this.dragging)
                    {
                        dropRect.width = 16;
                    }
                    if (dropRect.Contains(this.dragPosition))
                    {
                        dragOver = true;
                    }
                }

                line.Draw(hasScrollbar, dragOver);
            }
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                HierarchyProEditorGroupWindow.Selected = null;
                this.renaming = null;
                this.Repaint();
            }

            GUI.EndScrollView();
        }

        private void OnHierarchyChange()
        {
            this.Repaint();
        }

        private void TrackDragOnDrag(Vector2 start)
        {
            foreach (HierarchyProEditorGroupLine line in this.groupData)
            {
                if (line.Rect.Contains(start))
                {
                    this.dragging = line.Group;
                    HierarchyProEditorGroupWindow.Selected = line.Group;
                    this.Repaint();
                }
            }
        }

        private void TrackDragOnDragging(Vector2 start, Vector2 current)
        {
            this.dragPosition = current;
        }

        private void TrackDragOnDrop(Vector2 start, Vector2 drop)
        {
            if (this.dragging == null)
            {
                return;
            }

            this.Drop(this.dragging, drop);
            this.dragging = null;
        }
    }
}
