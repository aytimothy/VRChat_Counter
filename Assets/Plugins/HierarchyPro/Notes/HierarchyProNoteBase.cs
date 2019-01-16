namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class HierarchyProNoteBase : IHierarchyProNote
    {
        [SerializeField]
        private Texture icon;

        [SerializeField]
        private Color colorBackground = Color.white;

        [SerializeField]
        private string text;

        /// <inheritdoc />
        public Texture Icon { get { return this.icon; } set { this.icon = value; } }

        /// <inheritdoc />
        public string Text { get { return this.text; } set { this.text = value; } }

        /// <inheritdoc />
        public Color ColorBackground { get { return this.colorBackground; } set { this.colorBackground = value; } }
    }
}
