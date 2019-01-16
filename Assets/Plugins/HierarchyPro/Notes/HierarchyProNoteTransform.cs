namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEngine;

    [Serializable]
    public class HierarchyProNoteTransform : HierarchyProNoteBase
    {
        [SerializeField]
        private Transform transform;

        public HierarchyProNoteTransform(Transform transform)
        {
            this.transform = transform;
        }

        public Transform Transform { get { return this.transform; } set { this.transform = value; } }
    }
}
