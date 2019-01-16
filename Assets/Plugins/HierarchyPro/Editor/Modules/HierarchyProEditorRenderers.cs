namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class HierarchyProEditorRenderers : HierarchyProEditorModuleBase
    {
        private IEnumerable<Renderer> renderers;
        private MeshFilter meshFilter;

        /// <inheritdoc />
        public HierarchyProEditorRenderers(HierarchyProEditor editor)
            : base(editor)
        {
        }

        public override void Draw(Rect rect)
        {
            int count = this.renderers.Count();
            if (count <= 0)
            {
                this.DrawMeshFilter(rect);

                return;
            }

            Renderer renderer = this.renderers.First();
            Texture icon = HierarchyProEditorIcons.GetComponentIcon(renderer.GetType());
            Rect iconRect = rect.GetCenteredIconRect(icon);
            GUI.DrawTexture(iconRect, icon);

            if (count > 1)
            {
                Rect plusRect = rect.GetCenteredIconRect(HierarchyProEditorIcons.Plus, false);
                GUI.color = new Color(1, 0, 0);
                GUI.DrawTexture(plusRect, HierarchyProEditorIcons.Plus);
                GUI.color = Color.white;
            }
        }

        public override void Update()
        {
            this.renderers = this.GetComponents<Renderer>().ToList();
            this.meshFilter = this.GetComponent<MeshFilter>();
        }

        private void DrawMeshFilter(Rect rect)
        {
            if (this.meshFilter == null)
            {
                return;
            }

            string tooltip = "There are no Renderers, but a MeshFilter is present.";
            if (this.meshFilter.sharedMesh == null)
            {
                tooltip += "\nThe MeshFilter has no Mesh assigned.";
            }

            Texture icon = HierarchyProEditorIcons.GetComponentIcon<MeshFilter>();
            Rect rectMeshFilter = rect.GetCenteredIconRect(icon);
            GUI.color = this.meshFilter.sharedMesh != null ? new Color(0.8f, 0.8f, 0.8f, 0.5f) : new Color(0.8f, 0, 0, 0.5f);
            GUI.Label(rectMeshFilter, new GUIContent(icon, tooltip), GUIStyle.none);
            GUI.color = Color.white;
        }
    }
}
