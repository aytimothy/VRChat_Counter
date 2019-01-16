namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public struct HierarchyProEditorGroupLine
    {
        private readonly HierarchyProGroup group;
        private readonly Rect rect;
        private readonly int indent;
        private List<Transform> transforms;
        private List<RectTransform> rectTransforms;

        private HierarchyProEditorGroupLine(HierarchyProGroup group, Rect rect, int indent)
            : this()
        {
            this.group = group;
            this.rect = rect;
            this.indent = indent;

            this.transforms = new List<Transform>();
            this.rectTransforms = new List<RectTransform>();
        }

        public static HierarchyProEditorGroupLine Create(HierarchyProGroup group, Rect rect, int indent)
        {
            HierarchyProEditorGroupLine groupLine = new HierarchyProEditorGroupLine(group, rect, indent);
            groupLine.Initialize();
            return groupLine;
        }

        public string Name { get { return this.group.Name; } }

        public HierarchyProGroup Group { get { return this.group; } }

        public Rect Rect { get { return this.rect; } }

        public int Indent { get { return this.indent; } }

        public Object[] GetObjects()
        {
            return this.AllTransforms
                       .Select(x => x.gameObject)
                       .Cast<Object>()
                       .ToArray();
        }

        public IEnumerable<Transform> AllTransforms { get { return this.group.Transforms.Where(x => (x != null) && (x.gameObject != null)); } }

        public int TransformCount { get { return this.Transforms.Count(); } }

        public int RectTransformCount { get { return this.RectTransforms.Count(); } }

        public IEnumerable<Transform> Transforms { get { return this.transforms; } }

        public IEnumerable<RectTransform> RectTransforms { get { return this.rectTransforms; } }

        public void Initialize()
        {
            foreach (Transform transform in this.AllTransforms)
            {
                RectTransform rectTransform = transform.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    this.rectTransforms.Add(rectTransform);
                }
                else
                {
                    this.transforms.Add(transform);
                }
            }
        }

        public int Count<T>()
            where T : Component
        {
            return this.group.Count<T>();
        }

        public void Draw(bool hasScrollbar, bool dragOver)
        {
            Rect lineRect = this.Rect;
            if (hasScrollbar)
            {
                lineRect.width -= 18;
            }

            if (HierarchyProEditorGroupWindow.Renaming == this.Group)
            {
                HierarchyProEditorGroupWindow.RenamingName = GUI.TextField(new Rect(lineRect) {x = lineRect.x + 16, width = lineRect.width - 16}, HierarchyProEditorGroupWindow.RenamingName);
                if ((Event.current.keyCode == KeyCode.Return) || (Event.current.keyCode == KeyCode.KeypadEnter))
                {
                    this.Group.Name = HierarchyProEditorGroupWindow.RenamingName;
                    HierarchyProEditorGroupWindow.Renaming = null;
                    HierarchyProEditorGroupWindow.Redraw();
                }
                if (Event.current.keyCode == KeyCode.Escape)
                {
                    HierarchyProEditorGroupWindow.Renaming = null;
                    HierarchyProEditorGroupWindow.Redraw();
                }
                return;
            }

            if (lineRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.keyCode == KeyCode.Delete)
                {
                    HierarchyProGroupLibrary.Delete(this.Group);
                    Event.current.Use();
                }
            }

            if (HierarchyProEditorGroupWindow.Selected == this.Group)
            {
                EditorGUI.DrawRect(lineRect, GUI.skin.settings.selectionColor);
            }

            Rect labelRect = new Rect(lineRect) {x = 16 + (this.Indent * 16)};
            if (this.group.HasChildren)
            {
                this.group.ShowChildren = EditorGUI.Foldout(new Rect(labelRect) {x = labelRect.x - 16, width = 16}, this.group.ShowChildren, GUIContent.none);
            }
            GUI.Label(labelRect, this.Name);

            bool collapseCount = HierarchyProEditorGroupWindow.Width <= 225;
            Rect rectActive = new Rect(lineRect) {x = lineRect.xMax - 16, width = 16};
            Rect rectLocked = new Rect(rectActive) {x = rectActive.x - 16};
            Rect rectNotes = new Rect(rectLocked) {x = rectLocked.x - 12};
            Rect rectDivider1 = new Rect(rectNotes) {x = rectNotes.x - 2, width = 1};
            Rect rectCount = new Rect(lineRect) {x = rectDivider1.x - 56, width = 56};
            Rect rectDivider2 = new Rect(rectCount) {x = rectCount.x - 4, width = 1};
            Rect rectSelect = new Rect(lineRect) {x = (collapseCount ? rectDivider1 : rectDivider2).x - 12, width = 12};
            Rect rectAddRemove = new Rect(rectActive) {x = rectSelect.x - 24, width = 24};
            Rect rectControlArea = new Rect(lineRect) {xMin = rectAddRemove.x, xMax = rectActive.xMax};

            EditorGUI.DrawRect(rectControlArea, HierarchyProEditorColors.Background);

            this.DrawActive(rectActive);
            this.DrawLocked(rectLocked);
            HierarchyProEditorNotes.Draw(rectNotes, this.group);

            EditorGUI.DrawRect(rectDivider1, EditorGUIUtility.isProSkin ? new Color(0.1f, 0.1f, 0.1f) : new Color(0.6f, 0.6f, 0.6f));

            if (!collapseCount)
            {
                this.DrawCount(rectCount);
                EditorGUI.DrawRect(rectDivider2, EditorGUIUtility.isProSkin ? new Color(0.1f, 0.1f, 0.1f) : new Color(0.6f, 0.6f, 0.6f));
            }

            this.DrawSelect(rectSelect);
            this.DrawAddRemove(rectAddRemove);

            if (GUI.Button(new Rect(labelRect) {width = rectSelect.x - labelRect.x}, GUIContent.none, GUIStyle.none))
            {
                HierarchyProEditorGroupWindow.Selected = this.Group;
                if (Event.current.button == 1)
                {
                    this.ShowContextMenu();
                }
            }

            if (dragOver)
            {
                if (HierarchyProEditorGroupWindow.Dragging == this.group)
                {
                    GUI.Label(new Rect(lineRect) {width = 16}, GUIContent.none, HierarchyProEditorStyles.DropHighlight);
                }
                else
                {
                    GUI.Label(lineRect, GUIContent.none, HierarchyProEditorStyles.DropHighlight);
                }
            }
        }

        private void DrawActive(Rect rect)
        {
            Texture icon = this.GetActiveIcon();
            Rect check = rect.GetCenteredIconRect(icon);
            check.y += 2;
            GUI.DrawTexture(check, icon);

            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                this.Group.ToggleActive();
            }
        }

        private void DrawAddRemove(Rect right)
        {
            float width = right.width / 2;
            Rect rect = new Rect(right) {x = right.x + width, width = width};
            Texture icon = HierarchyProEditorIcons.Plus;
            Rect iconRect = rect.GetCenteredIconRect(icon);
            GUI.DrawTexture(iconRect, icon);
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                HierarchyProEditorGroupLine.SelectionAdd(this.Group);
            }

            rect = new Rect(rect) {x = right.x};
            icon = HierarchyProEditorIcons.Minus;
            iconRect = rect.GetCenteredIconRect(icon);
            GUI.DrawTexture(iconRect, icon);
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                HierarchyProEditorGroupLine.SelectionRemove(this.Group);
            }
        }

        private void DrawCount(Rect rect)
        {
            float iconSize = 12;
            float labelWidth = (rect.width - (iconSize * 2)) / 2;

            Texture iconTransform = HierarchyProEditorIcons.GetComponentIcon<Transform>();
            Rect rectTransform = new Rect(rect) {width = iconSize + labelWidth};
            Rect rectIconTransform = new Rect(rectTransform) {width = iconSize};
            Rect rectLabelTransform = new Rect(rectTransform) {x = rectTransform.x + iconSize, width = labelWidth};

            if (this.TransformCount > 0)
            {
                GUI.DrawTexture(rectIconTransform.GetCenteredIconRect(iconTransform, iconSize, iconSize), iconTransform);
                GUI.Label(rectLabelTransform, this.TransformCount.ToString(), HierarchyProEditorStyles.LabelTinyCentered);
            }
            if (GUI.Button(rectTransform, GUIContent.none, GUIStyle.none))
            {
                Selection.objects = this.Transforms.Where(x => (x != null) && (x.gameObject != null)).Select(x => x.gameObject).Cast<Object>().ToArray();
            }

            Texture iconRectTransform = HierarchyProEditorIcons.GetComponentIcon<RectTransform>();
            Rect rectRectTransform = new Rect(rect) {width = iconSize + labelWidth, x = rect.x + iconSize + labelWidth};
            Rect rectIconRectTransform = new Rect(rectRectTransform) {width = iconSize};
            Rect rectLabelRectTransform = new Rect(rectRectTransform) {x = rectRectTransform.x + iconSize, width = labelWidth};

            if (this.RectTransformCount > 0)
            {
                GUI.DrawTexture(rectIconRectTransform.GetCenteredIconRect(iconRectTransform, iconSize, iconSize), iconRectTransform);
                GUI.Label(rectLabelRectTransform, this.RectTransformCount.ToString(), HierarchyProEditorStyles.LabelTinyCentered);
            }
            if (GUI.Button(rectRectTransform, GUIContent.none, GUIStyle.none))
            {
                Selection.objects = this.RectTransforms.Where(x => (x != null) && (x.gameObject != null)).Select(x => x.gameObject).Cast<Object>().ToArray();
            }
        }

        private void DrawLocked(Rect rect)
        {
            bool locked = this.Group.LockedAny;
            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                this.Group.ToggleLock();
                SceneView.RepaintAll();
                EditorApplication.RepaintHierarchyWindow();
            }
            HierarchyProEditorIcons.IconPair iconLocked = locked ? HierarchyProEditorIcons.LockedOn : HierarchyProEditorIcons.LockedOff;
            Rect check = rect.GetCenteredIconRect(iconLocked);
            GUI.DrawTexture(check, iconLocked);
        }

        private void DrawSelect(Rect right)
        {
            Texture icon = HierarchyProEditorIcons.Select;
            Rect iconRect = right.GetCenteredIconRect(icon, 12, 12);
            GUI.DrawTexture(iconRect, icon);

            if (GUI.Button(right, GUIContent.none, GUIStyle.none))
            {
                Selection.objects = this.GetObjects();
            }
        }

        private static void SelectionAdd(object userdata)
        {
            HierarchyProGroup group = (HierarchyProGroup) userdata;
            group.AddObjects(Selection.objects);
        }

        private static void SelectionRemove(object userdata)
        {
            HierarchyProGroup group = (HierarchyProGroup) userdata;
            group.RemoveObjects(Selection.objects);
        }

        private void ShowContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Selection"), false, HierarchyProEditorGroupLine.SelectionAdd, this.group);
            menu.AddItem(new GUIContent("Remove Selection"), false, HierarchyProEditorGroupLine.SelectionRemove, this.group);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Rename"), false, HierarchyProEditorGroupLine.Rename, this.group);
            menu.AddItem(new GUIContent("Delete"), false, HierarchyProEditorGroupLine.Delete, this.group);
            menu.ShowAsContext();
        }

        private static void Rename(object userdata)
        {
            HierarchyProGroup group = (HierarchyProGroup) userdata;
            HierarchyProEditorGroupWindow.Renaming = group;
            HierarchyProEditorGroupWindow.RenamingName = group.Name;
        }

        private static void Delete(object userdata)
        {
            HierarchyProGroup group = (HierarchyProGroup) userdata;
            HierarchyProGroupLibrary.Delete(group);
            HierarchyProEditorGroupWindow.Redraw();
        }

        private Texture GetActiveIcon()
        {
            switch (this.group.ActiveState)
            {
                default:
                case HierarchyProGroupActiveState.Enabled:
                    return HierarchyProEditorIcons.CheckmarkOn;
                case HierarchyProGroupActiveState.Disabled:
                    return HierarchyProEditorIcons.CheckmarkOff;
                case HierarchyProGroupActiveState.Partial:
                    return HierarchyProEditorIcons.CheckmarkPartial;
            }
        }
    }
}
