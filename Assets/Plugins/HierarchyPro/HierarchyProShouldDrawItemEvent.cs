namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEngine;

    public delegate void HierarchyProShouldDrawItemDelegate(object sender, HierarchyProShouldDrawItemEventArgs e);

    public class HierarchyProShouldDrawItemEventArgs : EventArgs
    {
        private readonly GameObject gameObject;
        private readonly Rect rect;
        private bool cancel;

        public HierarchyProShouldDrawItemEventArgs(GameObject gameObject, Rect rect)
        {
            this.gameObject = gameObject;
            this.rect = rect;
        }

        public bool Cancel { get { return this.cancel; } set { this.cancel = value; } }

        public GameObject GameObject { get { return this.gameObject; } }

        public Rect Rect { get { return this.rect; } }
    }
}
