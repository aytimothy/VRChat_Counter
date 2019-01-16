namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public static class HierarchyProEditorNotes
    {
        public static void Draw(Rect rect, Transform transform)
        {
            IHierarchyProNote note = HierarchyProNotesLibrary.Find(transform);
            bool clicked = HierarchyProEditorNotes.Draw(rect, note);

            if (clicked)
            {
                if (note == null)
                {
                    note = HierarchyProNotesLibrary.Create(transform);
                    note.Icon = HierarchyProEditorIcons.Note;
                }

                HierarchyProEditorNoteWindow content = new HierarchyProEditorNoteWindow(note);
                PopupWindow.Show(rect, content);
            }
        }

        public static void Draw(Rect rect, HierarchyProGroup group)
        {
            IHierarchyProNote note = HierarchyProNotesLibrary.Find(group);
            bool clicked = HierarchyProEditorNotes.Draw(rect, note);

            if (clicked)
            {
                if (note == null)
                {
                    note = HierarchyProNotesLibrary.Create(group);
                    note.Icon = HierarchyProEditorIcons.Note;
                }

                HierarchyProEditorNoteWindow content = new HierarchyProEditorNoteWindow(note);
                PopupWindow.Show(rect, content);
            }
        }

        public static void Draw(Rect rect, GameObject gameObject)
        {
            HierarchyProEditorNotes.Draw(rect, gameObject.transform);
        }

        private static bool Draw(Rect rect, IHierarchyProNote note)
        {
            Texture icon = HierarchyProEditorIcons.Plus;
            Color color = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.1f) : new Color(1, 1, 1, 0.2f);
            if (note != null)
            {
                icon = note.Icon;
                color = note.ColorBackground;
            }

            if (icon != null)
            {
                GUI.color = color;
                GUI.DrawTexture(rect.GetCenteredIconRect(icon), icon);
                GUI.color = Color.white;
            }

            return GUI.Button(rect, GUIContent.none, GUIStyle.none);
        }
    }
}
