using UnityEditor;
using UnityEngine;
using UntitledGames.Hierarchy;

public class HierarchyProPreferences : EditorWindow
{
    private static bool showComponents;
    private static bool showGizmos;
    private static bool showLayersTags;

    public static bool ShowComponents
    {
        get { return HierarchyProPreferences.showComponents; }
        set
        {
            HierarchyProPreferences.showComponents = value;
            HierarchyProPreferences.SetBool("showComponents", value);
            HierarchyPro.Redraw();
        }
    }

    public static bool ShowGizmos
    {
        get { return HierarchyProPreferences.showGizmos; }
        set
        {
            HierarchyProPreferences.showGizmos = value;
            HierarchyProPreferences.SetBool("showGizmos", value);
            HierarchyPro.Redraw();
        }
    }

    public static bool ShowLayersTags
    {
        get { return HierarchyProPreferences.showLayersTags; }
        set
        {
            HierarchyProPreferences.showLayersTags = value;
            HierarchyProPreferences.SetBool("showLayersTags", value);
            HierarchyPro.Redraw();
        }
    }

    public static void Load()
    {
        HierarchyProPreferences.showGizmos = HierarchyProPreferences.GetBool("showGizmos", true);
        HierarchyProPreferences.showComponents = HierarchyProPreferences.GetBool("showComponents", true);
        HierarchyProPreferences.showLayersTags = HierarchyProPreferences.GetBool("showLayersTags", true);
    }

    private static bool GetBool(string name, bool value)
    {
        return EditorPrefs.GetBool(string.Format("HierarchyPro.{0}.bool", name), value);
    }

    private static float GetFloat(string name, float value)
    {
        return EditorPrefs.GetFloat(string.Format("HierarchyPro.{0}.float", name), value);
    }

    private static int GetInt(string name, int value)
    {
        return EditorPrefs.GetInt(string.Format("HierarchyPro.{0}.int", name), value);
    }

    [PreferenceItem("HierarchyPro")]
    private static void OnPreferencesGUI()
    {
        EditorGUILayout.BeginHorizontal(HierarchyProEditorStyles.ToolbarHorizontal);
        EditorGUILayout.LabelField("Enable/Disable Modules");
        EditorGUILayout.EndHorizontal();
        HierarchyProPreferences.ShowGizmos = EditorGUILayout.Toggle("Gizmos", HierarchyProPreferences.ShowGizmos);
        HierarchyProPreferences.ShowComponents = EditorGUILayout.Toggle("Components", HierarchyProPreferences.ShowComponents);
        HierarchyProPreferences.ShowLayersTags = EditorGUILayout.Toggle("Layers / Tags", HierarchyProPreferences.ShowLayersTags);
    }

    private static void SetBool(string name, bool value)
    {
        EditorPrefs.SetBool(string.Format("HierarchyPro.{0}.bool", name), value);
    }

    private static void SetFloat(string name, float value)
    {
        EditorPrefs.SetFloat(string.Format("HierarchyPro.{0}.float", name), value);
    }

    private static void SetInt(string name, int value)
    {
        EditorPrefs.SetInt(string.Format("HierarchyPro.{0}.int", name), value);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("HierarchyPro", HierarchyProEditorStyles.PreferencesTitle, GUILayout.Height(48));

        HierarchyProPreferences.OnPreferencesGUI();
    }
}
