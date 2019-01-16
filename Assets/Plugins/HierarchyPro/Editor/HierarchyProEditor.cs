namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditor
    {
        private GameObject gameObject;
        private HierarchyProEditorStaticFlags staticFlags;
        private HierarchyProEditorComponents components;
        private HierarchyProEditorPrefabs prefabs;
        private HierarchyProEditorLayersTags layersTags;

        //private HierarchyProEditorRenderers renderers;
        //private HierarchyProEditorColliders colliders;

        public HierarchyProEditor(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.staticFlags = new HierarchyProEditorStaticFlags(this);
            this.components = new HierarchyProEditorComponents(this);
            this.prefabs = new HierarchyProEditorPrefabs(this);
            this.layersTags = new HierarchyProEditorLayersTags(this);

            //this.colliders = new HierarchyProEditorColliders(this);
            //this.renderers = new HierarchyProEditorRenderers(this);
        }

        private HierarchyProEditor()
        {
        }

        public GameObject GameObject { get { return this.gameObject; } }
        public bool HasChildren { get { return this.GameObject.transform.childCount > 0; } }

        private static void RepaintInspector()
        {
            Editor[] editors = Resources.FindObjectsOfTypeAll<Editor>();
            foreach (Editor editor in editors)
            {
                editor.Repaint();
            }
        }

        public void Draw(Rect rect)
        {
            int labelWidth = 140;
            int widthRequiredForLayers = 250;

            bool selected = Selection.gameObjects.Contains(this.gameObject);

            this.Update();

            //Color backgroundColor = EditorGUIUtility.isProSkin
            //? new Color32(56, 56, 56, 255)
            //: new Color32(194, 194, 194, 255);
            //EditorGUI.DrawRect(rect, backgroundColor);
            //EditorGUI.DrawRect(new Rect(rect) {x = 0, width = 16}, backgroundColor);

            //Rect label = new Rect(rect) {x = rect.x - 2, y = rect.y};
            //GUI.Label(label, this.gameObject.name);

            bool collapseLayers = (HierarchyProEditorWindow.Width < widthRequiredForLayers) || !HierarchyProPreferences.ShowLayersTags;

            Rect rectGizmo = new Rect(rect) {x = 0, width = 16};
            Rect rectActive = new Rect(rect) {xMin = rect.xMax - 16, width = 16};
            Rect rectLocked = new Rect(rectActive) {x = rectActive.x - 16, width = 16};
            Rect rectNotes = new Rect(rectLocked) {x = rectLocked.x - 12, width = 16};
            Rect rectPrefab = new Rect(rectNotes) {x = rectNotes.x - 16, width = 16};
            Rect rectStatic = new Rect(rectPrefab) {x = rectPrefab.x - 23, width = 21};
            Rect rectLayers = new Rect(rectStatic) {x = rectStatic.x - 64, width = 64};
            Rect rectDivider = new Rect(rectLayers) {x = (collapseLayers ? rectStatic : rectLayers).x - 4, width = 1, y = rect.y + 2};

            Rect rectControls = new Rect(rect) {xMin = rectDivider.xMin, xMax = rectActive.xMax};
            EditorGUI.DrawRect(rectControls, HierarchyProEditorColors.Background);

            if (HierarchyProPreferences.ShowGizmos)
            {
                this.DrawGizmo(rectGizmo);
            }

            this.DrawActive(rectActive);
            this.DrawLocked(rectLocked);
            HierarchyProEditorNotes.Draw(rectNotes, this.GameObject);
            this.prefabs.Draw(rectPrefab);
            this.staticFlags.Draw(rectStatic);

            if (!collapseLayers)
            {
                this.layersTags.Draw(rectLayers);
            }

            EditorGUI.DrawRect(rectDivider, EditorGUIUtility.isProSkin ? new Color(0.1f, 0.1f, 0.1f) : new Color(0.6f, 0.6f, 0.6f));

            if (HierarchyProPreferences.ShowComponents)
            {
                int componentCount = this.components.Count;
                componentCount = Mathf.Clamp(componentCount, 0, 16);

                float size = componentCount * 16;
                Rect rectComponent = new Rect(rectDivider) {x = rectDivider.x - (size + 2), width = size};
                Rect rectBlanker = new Rect(rectComponent) {x = rectComponent.x - 2, y = rectComponent.y - 2, width = rectComponent.width + 4, height = rectComponent.height};
                rectComponent.y -= 2;
                EditorGUI.DrawRect(rectBlanker, HierarchyProEditorColors.Background);
                this.components.MaximumComponents = componentCount;
                this.components.Draw(rectComponent);
            }
        }

        public T GetComponent<T>()
        {
            return this.components.GetComponents<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>()
        {
            return this.components.GetComponents<T>();
        }

        public void Update()
        {
            //this.treeDepth = this.GetTreeDepth();
            // Not used, some cost. Default rect x can be used - reliably? not so sure.

            this.staticFlags.Update();
            this.components.Update();
            this.prefabs.Update();
            this.layersTags.Update();
            //this.folders.Update();
            //this.colliders.Update();
            //this.renderers.Update();
        }

        private void DrawActive(Rect rect)
        {
            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                this.gameObject.SetActive(!this.gameObject.activeSelf);
            }
            HierarchyProEditorIcons.IconPair iconEnabled = this.gameObject.activeSelf ? HierarchyProEditorIcons.CheckmarkOn : HierarchyProEditorIcons.CheckmarkOff;
            Rect check = rect.GetCenteredIconRect(iconEnabled);
            check.y += 2;
            GUI.color = !this.gameObject.activeInHierarchy && this.gameObject.activeSelf ? Color.gray : Color.white;
            GUI.DrawTexture(check, iconEnabled);
            GUI.color = Color.white;
        }

        private void DrawGizmo(Rect rect)
        {
            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                HierarchyProEditorReflection.ShowIconSelectorForObject(this.gameObject, rect);
            }

            Texture2D icon = HierarchyProEditorReflection.GetIcon(this.gameObject);
            if (icon != null)
            {
                Rect iconRect = rect.GetCenteredIconRect(icon);
                try
                {
                    if (icon.name.StartsWith("sv_icon_dot"))
                    {
                        string woGizmo = icon.name.Substring(0, icon.name.Length - 12);
                        int dotType = int.Parse(woGizmo.Substring(11));
                        if (dotType <= 7)
                        {
                            iconRect = rect.GetCenteredIconRect(icon, 8, 8);
                        }
                        else
                        {
                            iconRect = rect.GetCenteredIconRect(icon, 12, 12);
                        }
                    }
                    else if (icon.name.StartsWith("sv_label_"))
                    {
                        iconRect = rect.GetCenteredIconRect(icon, 14, 6);
                    }
                }
                catch
                {
                    // ignored
                }
                GUI.DrawTexture(iconRect, icon);
            }
        }

        private void DrawLocked(Rect rect)
        {
            bool locked = this.gameObject.hideFlags.HasFlag(HideFlags.NotEditable);
            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                this.gameObject.hideFlags ^= HideFlags.NotEditable;
                SceneView.RepaintAll();
                HierarchyProEditor.RepaintInspector();
            }
            HierarchyProEditorIcons.IconPair iconLocked = locked ? HierarchyProEditorIcons.LockedOn : HierarchyProEditorIcons.LockedOff;
            Rect check = rect.GetCenteredIconRect(iconLocked);
            GUI.color = !this.gameObject.activeInHierarchy && this.gameObject.activeSelf ? Color.gray : Color.white;
            GUI.DrawTexture(check, iconLocked);
            GUI.color = Color.white;
        }

        private int GetTreeDepth()
        {
            int safety = 256;
            bool found = false;
            int treeDepth = 0;
            Transform transform = this.gameObject.transform;
            do
            {
                if (transform.parent == null)
                {
                    found = true;
                }
                else
                {
                    transform = transform.parent;
                    treeDepth++;
                }
            } while (!found && (safety > 0));
            return treeDepth;
        }
    }
}
