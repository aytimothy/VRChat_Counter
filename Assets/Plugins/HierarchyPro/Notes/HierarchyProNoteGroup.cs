namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEngine;

    [Serializable]
    public class HierarchyProNoteGroup : HierarchyProNoteBase
    {
        [SerializeField]
        private long groupID;

        public HierarchyProNoteGroup(HierarchyProGroup group)
        {
            this.GroupID = group.ID;
        }

        public HierarchyProGroup Group { get { return HierarchyProGroupLibrary.Find(this.GroupID); } }

        public long GroupID { get { return this.groupID; } set { this.groupID = value; } }
    }
}
