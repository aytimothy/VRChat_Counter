namespace UntitledGames.Hierarchy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class HierarchyProGroupLibrary : MonoBehaviour, IEnumerable<HierarchyProGroup>
    {
        private static HierarchyProGroupLibrary instance;

        [SerializeField]
        private HierarchyProGroup[] groups;

        [SerializeField]
        private int selectedID;

        public static int Count { get { return HierarchyProGroupLibrary.Groups.Count(); } }

        public static IEnumerable<HierarchyProGroup> Groups { get { return HierarchyProGroupLibrary.Instance.groups ?? (HierarchyProGroupLibrary.Instance.groups = new HierarchyProGroup[0]); } }

        public static HierarchyProGroupLibrary Instance
        {
            get
            {
                if ((HierarchyProGroupLibrary.instance == null) || (HierarchyProGroupLibrary.instance.gameObject == null))
                {
                    HierarchyProGroupLibrary.FindInstance();
                }
                return HierarchyProGroupLibrary.instance;
            }
        }

        public static IEnumerable<HierarchyProGroup> RootGroups { get { return HierarchyProGroupLibrary.Groups.Where(x => x.ParentID == 0); } }

        public static HierarchyProGroup Selected { get { return HierarchyProGroupLibrary.Groups.FirstOrDefault(x => x.ID == HierarchyProGroupLibrary.SelectedID); } }

        public static int SelectedID { get { return HierarchyProGroupLibrary.Instance.selectedID; } set { HierarchyProGroupLibrary.Instance.selectedID = value; } }

        /// <inheritdoc />
        public IEnumerator<HierarchyProGroup> GetEnumerator()
        {
            return ((IEnumerable<HierarchyProGroup>) this.groups).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static void Add(HierarchyProGroup group)
        {
            HierarchyProGroupLibrary.Instance.groups = new List<HierarchyProGroup>(HierarchyProGroupLibrary.Groups) {group}.ToArray();
        }

        public static void Delete(HierarchyProGroup group)
        {
            List<HierarchyProGroup> groups = new List<HierarchyProGroup>(HierarchyProGroupLibrary.Instance.groups);
            groups.Remove(group);
            HierarchyProGroupLibrary.Instance.groups = groups.ToArray();
        }

        public static HierarchyProGroup Find(long id)
        {
            return HierarchyProGroupLibrary.Groups.FirstOrDefault(x => x.ID == id);
        }

        public static IEnumerable<HierarchyProGroup> FindChildren(HierarchyProGroup group)
        {
            if (group == null)
            {
                return Enumerable.Empty<HierarchyProGroup>();
            }
            return HierarchyProGroupLibrary.Groups.Where(x => (x.Parent != null) && (x.Parent.ID == group.ID));
        }

        public static HierarchyProGroupLibrary FindInstance()
        {
            if ((HierarchyProGroupLibrary.instance != null) && (HierarchyProGroupLibrary.instance.gameObject != null))
            {
                return HierarchyProGroupLibrary.instance;
            }

            HierarchyProGroupLibrary.instance = Object.FindObjectOfType<HierarchyProGroupLibrary>();
            if ((HierarchyProGroupLibrary.instance != null) && (HierarchyProGroupLibrary.instance.gameObject != null))
            {
                return HierarchyProGroupLibrary.instance;
            }

            GameObject gameObject = GameObject.Find("HierarchyPro Data") ?? new GameObject("HierarchyPro Data");
            gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy;
            HierarchyProGroupLibrary library = gameObject.AddComponent<HierarchyProGroupLibrary>();
            HierarchyProGroupLibrary.instance = library;
            return HierarchyProGroupLibrary.instance;
        }

        public static int GetID()
        {
            int nextID;
            int failsafe = 8192;
            do
            {
                nextID = Guid.NewGuid().GetHashCode();
                failsafe--;
                if (failsafe <= 0)
                {
                    throw new Exception("[HierarchyPro] Could not generate a group ID within a reasonable number of tries.\nThis may happen due to probability in exceptional circumstances. Please try again.");
                }
            } while (HierarchyProGroupLibrary.Groups.Any(x => (x != null) && (x.ParentID == nextID)));
            return nextID;
        }

        private void OnValidate()
        {
            HierarchyPro.Redraw();
        }
    }
}
