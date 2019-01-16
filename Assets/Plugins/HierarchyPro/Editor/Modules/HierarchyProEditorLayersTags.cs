namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditorLayersTags : HierarchyProEditorModuleBase
    {
        private string tag;
        private string layer;
        private bool tagError;
        private bool layerError;

        /// <inheritdoc />
        public HierarchyProEditorLayersTags(HierarchyProEditor editor)
            : base(editor)
        {
        }

        public string Layer { get { return this.layer; } }

        public int LayerID { get { return this.GameObject.layer; } set { this.GameObject.layer = value; } }

        public string Tag { get { return this.tag; } set { this.tag = value; } }

        /// <inheritdoc />
        public override void Draw(Rect rect)
        {
            bool hasCustomLayer = this.layer != "Default";
            bool hasCustomTag = this.tag != "Untagged";

            Rect layerRect = new Rect(rect);
            layerRect.height /= 2;

            Rect tagRect = new Rect(rect);
            tagRect.height /= 2;
            tagRect.y += tagRect.height;

            GUI.color = hasCustomLayer ? (this.layerError ? new Color(1, 0.4f, 0.4f) : Color.white) : (EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.1f) : new Color(1, 1, 1, 0.2f));
            if (GUI.Button(layerRect, this.layer, HierarchyProEditorStyles.LabelTiny))
            {
                this.ShowLayersMenu();
            }
            GUI.color = hasCustomTag ? (this.tagError ? new Color(1, 0.4f, 0.4f) : Color.white) : (EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.1f) : new Color(1, 1, 1, 0.2f));
            if (GUI.Button(tagRect, this.tag, HierarchyProEditorStyles.LabelTiny))
            {
                this.ShowTagsMenu();
            }
            GUI.color = Color.white;
        }

        public void ShowLayersMenu()
        {
            GenericMenu layersMenu = new GenericMenu();
            layersMenu.AddDisabledItem(new GUIContent(string.Format("Layer ({0})", this.GameObject.name)));
            layersMenu.AddSeparator("");
            layersMenu.AddItem(new GUIContent("Default"), this.LayerID == 0, this.LayerChanged, 0);
            layersMenu.AddSeparator("");
            bool finalSeperator = false;
            for (int layer = 1; layer < 32; layer++)
            {
                string layerName = LayerMask.LayerToName(layer);
                if (!string.IsNullOrEmpty(layerName))
                {
                    layersMenu.AddItem(new GUIContent(layerName), this.LayerID == layer, this.LayerChanged, layer);
                    finalSeperator = false;
                }
                if (layer == 7)
                {
                    layersMenu.AddSeparator("");
                    finalSeperator = true;
                }
            }
            if (!finalSeperator)
            {
                layersMenu.AddSeparator("");
            }
            layersMenu.AddDisabledItem(new GUIContent("Add Layer..."));
            layersMenu.ShowAsContext();
        }

        public void ShowTagsMenu()
        {
            GenericMenu layersMenu = new GenericMenu();
            layersMenu.AddDisabledItem(new GUIContent(string.Format("Tag ({0})", this.GameObject.name)));
            layersMenu.AddSeparator("");
            layersMenu.AddItem(new GUIContent("Untagged"), string.IsNullOrEmpty(this.Tag), this.TagChanged, "Untagged");
            layersMenu.AddSeparator("");
            bool finalSeperator = true;
            for (int tag = 0; tag < UnityEditorInternal.InternalEditorUtility.tags.Length; tag++)
            {
                string tagName = UnityEditorInternal.InternalEditorUtility.tags[tag];
                if (!string.IsNullOrEmpty(tagName))
                {
                    layersMenu.AddItem(new GUIContent(tagName), this.tag == tagName, this.TagChanged, tagName);
                    finalSeperator = false;
                }
            }
            if (!finalSeperator)
            {
                layersMenu.AddSeparator("");
            }
            layersMenu.AddDisabledItem(new GUIContent("Add Tag..."));
            layersMenu.ShowAsContext();
        }

        /// <inheritdoc />
        public override void Update()
        {
            this.layer = LayerMask.LayerToName(this.GameObject.layer);
            this.layerError = string.IsNullOrEmpty(this.layer);
            if (this.layerError)
            {
                this.layer = string.Format("Layer {0}", this.GameObject.layer);
            }
            try
            {
                this.tag = this.GameObject.tag;
            }
            catch
            {
                this.tag = string.Empty;
            }
            this.tagError = string.IsNullOrEmpty(this.tag);
            if (this.tagError)
            {
                this.tag = "Missing Tag";
            }
        }

        private void LayerChanged(object userdata)
        {
            this.LayerID = (int) userdata;
        }

        private void TagChanged(object userdata)
        {
            this.GameObject.tag = (string) userdata;
        }
    }
}
