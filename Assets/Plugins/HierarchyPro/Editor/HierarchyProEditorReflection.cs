namespace UntitledGames.Hierarchy
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class HierarchyProEditorReflection
    {
        private static Assembly assemblyUnityEditor;
        private static Type editorGUIUtility;
        private static MethodInfo editorGUIUtility_GetIconForObject;
        private static Type iconSelector;
        private static MethodInfo iconSelector_ShowAtPosition;

        public static Texture2D GetIcon(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            Texture2D icon = null;
            try
            {
                icon = HierarchyProEditorReflection.editorGUIUtility_GetIconForObject.Invoke(null, new object[] {gameObject}) as Texture2D;
            }
            catch (Exception)
            {
                // ignored
            }
            return icon;
        }

        public static void Load()
        {
            // Assemblies
            HierarchyProEditorReflection.assemblyUnityEditor = Assembly.Load("UnityEditor");

            // Types
            //HierarchyProEditorReflection.editorApplication = typeof(EditorApplication);
            HierarchyProEditorReflection.editorGUIUtility = typeof(EditorGUIUtility);
            HierarchyProEditorReflection.iconSelector = HierarchyProEditorReflection.assemblyUnityEditor.GetType("UnityEditor.IconSelector");
            //HierarchyProEditorReflection.tagManager = HierarchyProEditorReflection.assemblyUnityEditor.GetType("UnityEditor.TagManager");

            // Properties
            //HierarchyProEditorReflection.editorApplication_TagManager = HierarchyProEditorReflection.editorApplication.GetProperty("tagManager", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty);

            // Methods
            HierarchyProEditorReflection.editorGUIUtility_GetIconForObject = HierarchyProEditorReflection.editorGUIUtility.GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            HierarchyProEditorReflection.iconSelector_ShowAtPosition = HierarchyProEditorReflection.iconSelector.GetMethod("ShowAtPosition", BindingFlags.Static | BindingFlags.NonPublic);

            // Objects
            //HierarchyProEditorReflection.instance_TagManager = HierarchyProEditorReflection.editorApplication_TagManager.GetValue(null, null);
        }

        public static void ShowIconSelectorForObject(GameObject gameObject, Rect rect)
        {
            HierarchyProEditorReflection.iconSelector_ShowAtPosition.Invoke(null, new object[] {gameObject, rect, true});
            // Third paramater is "showLabelIcons"
        }
    }
}
