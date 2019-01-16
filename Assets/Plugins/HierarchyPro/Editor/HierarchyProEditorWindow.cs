namespace UntitledGames.Hierarchy
{
    using UnityEditor;

    public static class HierarchyProEditorWindow
    {
        private static EditorWindow editorWindow;
        public static EditorWindow EditorWindow { get { return HierarchyProEditorWindow.editorWindow ?? (HierarchyProEditorWindow.editorWindow = HierarchyProEditorWindow.GetWindow()); } }

        public static float Height { get { return HierarchyProEditorWindow.EditorWindow.position.height; } }

        public static float Width { get { return HierarchyProEditorWindow.EditorWindow.position.width; } }

        private static EditorWindow GetWindow()
        {
            if (HierarchyProEditorWindow.editorWindow != null)
            {
                return HierarchyProEditorWindow.editorWindow;
            }

            EditorWindow resetWindow = EditorWindow.focusedWindow;
            EditorApplication.ExecuteMenuItem("Window/Hierarchy");
            HierarchyProEditorWindow.editorWindow = EditorWindow.focusedWindow;
            if (HierarchyProEditorWindow.editorWindow == null)
            {
                return null;
            }

            if (resetWindow != null)
            {
                resetWindow.Focus();
            }

            return HierarchyProEditorWindow.editorWindow;
        }
    }
}
