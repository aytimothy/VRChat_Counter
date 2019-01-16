namespace UntitledGames.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class HierarchyProGroup
    {
        [NonSerialized]
        private HierarchyProGroup parent;

        [SerializeField]
        private int id;

        [SerializeField]
        private bool showChildren = true;

        [SerializeField]
        private int parentID;

        [SerializeField]
        private string name;

        [SerializeField]
        private Transform[] transforms;

        [NonSerialized]
        private bool hasChildren;

        private HierarchyProGroup()
        {
        }

        public HierarchyProGroupActiveState ActiveState
        {
            get
            {
                int count = 0;
                bool previous = false;
                foreach (bool current in this.transforms.Where(x => (x != null) && (x.gameObject != null)).Select(transform => transform.gameObject.activeSelf))
                {
                    if ((count > 0) && (current != previous))
                    {
                        return HierarchyProGroupActiveState.Partial;
                    }
                    previous = current;
                    count++;
                }
                return previous ? HierarchyProGroupActiveState.Enabled : HierarchyProGroupActiveState.Disabled;
            }
        }

        public bool HasChildren { get { return this.hasChildren; } set { this.hasChildren = value; } }

        public int ID { get { return this.id; } }

        public bool LockedAny { get { return this.transforms.Any(x => (x != null) && (x.gameObject != null) && x.gameObject.hideFlags.HasFlag(HideFlags.NotEditable)); } }

        public string Name { get { return this.name; } set { this.name = value; } }

        public HierarchyProGroup Parent
        {
            get
            {
                if (this.ParentID == 0)
                {
                    return null;
                }
                return this.parent ?? (this.parent = HierarchyProGroupLibrary.Find(this.ParentID));
            }
            set
            {
                this.parent = value;
                this.parentID = this.parent == null ? 0 : this.parent.ID;
            }
        }

        public int ParentID { get { return this.parentID; } }

        public bool ShowChildren { get { return this.showChildren; } set { this.showChildren = value; } }

        public IEnumerable<Transform> Transforms { get { return this.transforms ?? (this.transforms = new Transform[0]); } }

        public static HierarchyProGroup Create()
        {
            HierarchyProGroup group = new HierarchyProGroup();
            group.id = HierarchyProGroupLibrary.GetID();
            group.parentID = 0;
            return group;
        }

        private static IEnumerable<Transform> GetTransforms(IEnumerable<Object> objects)
        {
            List<Transform> transforms = new List<Transform>();
            foreach (Object o in objects)
            {
                Transform transform = o as Transform;
                if (transform != null)
                {
                    transforms.Add(transform);
                    continue;
                }

                GameObject gameObject = o as GameObject;
                if (gameObject != null)
                {
                    transforms.Add(gameObject.transform);
                }
            }
            return transforms;
        }

        public void AddObjects(IEnumerable<Object> objects)
        {
            this.AddTransforms(HierarchyProGroup.GetTransforms(objects));
        }

        public void AddTransforms(IEnumerable<Transform> newTransforms)
        {
            List<Transform> transforms = new List<Transform>(this.Transforms);
            transforms.AddRange(newTransforms.Where(transform => !transforms.Contains(transform)));
            this.transforms = transforms.ToArray();
        }

        public int Count<T>()
            where T : Component
        {
            return this.transforms.Select(x => x.GetComponent<T>()).Count(x => x != null);
        }

        public void GenerateName()
        {
            if (!this.Transforms.Any())
            {
                this.Name = "Empty Group";
                return;
            }
            if (this.Transforms.Count() == 1)
            {
                this.Name = this.Transforms.First().name + " Group";
                return;
            }
            this.Name = "New Group";
        }

        public bool IsChildOf(HierarchyProGroup group)
        {
            HierarchyProGroup child = this;
            while (child.Parent != group)
            {
                if (child.Parent == null)
                {
                    return false;
                }
                child = child.Parent;
            }
            return true;
        }

        public void RemoveObjects(IEnumerable<Object> objects)
        {
            this.RemoveTransforms(HierarchyProGroup.GetTransforms(objects));
        }

        public void RemoveTransforms(IEnumerable<Transform> removeTransforms)
        {
            List<Transform> transforms = new List<Transform>(this.Transforms);
            foreach (Transform removeTransform in removeTransforms)
            {
                transforms.Remove(removeTransform);
            }
            this.transforms = transforms.ToArray();
        }

        public void SetActive(bool active)
        {
            foreach (Transform transform in this.transforms.Where(x => (x != null) && (x.gameObject != null)))
            {
                transform.gameObject.SetActive(active);
            }
        }

        public void ToggleActive()
        {
            switch (this.ActiveState)
            {
                case HierarchyProGroupActiveState.Enabled:
                    this.SetActive(false);
                    break;
                case HierarchyProGroupActiveState.Disabled:
                case HierarchyProGroupActiveState.Partial:
                    this.SetActive(true);
                    break;
            }
        }

        public void ToggleLock()
        {
            bool locked = this.LockedAny;
            foreach (Transform transform in this.transforms.Where(x => (x != null) && (x.gameObject != null)))
            {
                if (locked)
                {
                    transform.gameObject.hideFlags &= ~HideFlags.NotEditable;
                }
                else
                {
                    transform.gameObject.hideFlags |= HideFlags.NotEditable;
                }
            }
        }
    }
}
