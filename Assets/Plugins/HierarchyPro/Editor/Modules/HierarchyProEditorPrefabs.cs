namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditorPrefabs : HierarchyProEditorModuleBase
    {
        private Object prefab;
        private bool isRoot;

        /// <inheritdoc />
        public HierarchyProEditorPrefabs(HierarchyProEditor editor)
            : base(editor)
        {
        }

        /// <inheritdoc />
        public override void Draw(Rect rect)
        {
            if (this.prefab == null)
            {
                return;
            }

            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                Selection.activeObject = this.prefab;
            }

            GUI.color = this.isRoot ? Color.white : new Color(1, 1, 1, 0.1f);
            GUI.DrawTexture(rect, HierarchyProEditorIcons.Prefab);
        }

        /// <inheritdoc />
        public override void Update()
        {
            this.prefab = PrefabUtility.GetPrefabParent(this.GameObject);
            this.isRoot = PrefabUtility.FindRootGameObjectWithSameParentPrefab(this.GameObject) == this.GameObject;
            //this.isRoot = true;
        }
    }
}
