namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class HierarchyProEditorLoader
    {
        static HierarchyProEditorLoader()
        {
            EditorApplication.playmodeStateChanged += HierarchyProEditorLoader.Load;
            HierarchyProEditorLoader.Load();
        }

        private static void ItemOnGUI(int instanceid, Rect selectionrect)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceid);
            if (obj == null)
            {
                return;
            }

            GameObject gameObject = obj as GameObject;
            if (gameObject == null)
            {
                return;
            }

            HierarchyProShouldDrawItemEventArgs e = HierarchyPro.OnShouldDrawItem(gameObject, selectionrect);
            if (e.Cancel)
            {
                return;
            }

            HierarchyProEditorCache.Draw(gameObject, selectionrect);
        }

        private static void Load()
        {
            HierarchyProPreferences.Load();

            HierarchyProEditorCache.Initialize();
            HierarchyProEditorReflection.Load();
            HierarchyProEditorIcons.Load();
            HierarchyProEditorColors.Load();
            HierarchyProEditorStyles.Load();

            HierarchyProGroupLibrary.FindInstance();

            EditorApplication.update -= HierarchyProEditorLoader.Update;
            EditorApplication.hierarchyWindowChanged -= HierarchyProEditorLoader.WindowChanged;
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyProEditorLoader.ItemOnGUI;
            Undo.undoRedoPerformed -= HierarchyProEditorLoader.UndoRedoPerformed;

            EditorApplication.update += HierarchyProEditorLoader.Update;
            EditorApplication.hierarchyWindowChanged += HierarchyProEditorLoader.WindowChanged;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyProEditorLoader.ItemOnGUI;
            Undo.undoRedoPerformed += HierarchyProEditorLoader.UndoRedoPerformed;
        }

        private static void UndoRedoPerformed()
        {
            EditorApplication.RepaintHierarchyWindow();
        }

        private static void Update()
        {
            if (HierarchyPro.RedrawPending)
            {
                EditorApplication.RepaintHierarchyWindow();
            }
            if (HierarchyProEditorGroupWindow.RedrawPending)
            {
                HierarchyProEditorGroupWindow.Redraw();
            }
        }

        private static void WindowChanged()
        {
            // Consider trying this as a contender for min max width tracking reset
        }
    }
}
