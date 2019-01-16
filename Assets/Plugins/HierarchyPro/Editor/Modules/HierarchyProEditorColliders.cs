namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class HierarchyProEditorColliders : HierarchyProEditorModuleBase
    {
        private IEnumerable<Collider> colliders;

        /// <inheritdoc />
        public HierarchyProEditorColliders(HierarchyProEditor editor)
            : base(editor)
        {
        }

        public override void Draw(Rect rect)
        {
            int count = this.colliders.Count();
            if (count <= 0)
            {
                return;
            }

            Collider collider = this.colliders.First();
            Texture icon = HierarchyProEditorIcons.GetComponentIcon(collider.GetType());
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
            this.colliders = this.Editor.GetComponents<Collider>().ToList();
        }
    }
}
