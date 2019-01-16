namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class HierarchyProEditorModuleBase
    {
        private HierarchyProEditor editor;

        protected HierarchyProEditorModuleBase(HierarchyProEditor editor)
        {
            this.editor = editor;
        }

        private HierarchyProEditorModuleBase()
        {
        }

        public HierarchyProEditor Editor { get { return this.editor; } }
        public GameObject GameObject { get { return this.editor.GameObject; } }
        public Transform Transform { get { return this.GameObject.transform; } }

        public abstract void Draw(Rect rect);
        public abstract void Update();

        public virtual T GetComponent<T>()
        {
            return this.Editor.GetComponent<T>();
        }

        public virtual IEnumerable<T> GetComponents<T>()
        {
            return this.Editor.GetComponents<T>();
        }
    }
}
