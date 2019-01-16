namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class HierarchyProNotesLibrary : MonoBehaviour
    {
        private static HierarchyProNotesLibrary instance;

        [SerializeField]
        private HierarchyProNoteTransform[] transformNotes;

        [SerializeField]
        private HierarchyProNoteGroup[] groupNotes;

        public static IEnumerable<HierarchyProNoteGroup> GroupNotes { get { return HierarchyProNotesLibrary.Instance.groupNotes ?? (HierarchyProNotesLibrary.Instance.groupNotes = new HierarchyProNoteGroup[0]); } }

        public static HierarchyProNotesLibrary Instance
        {
            get
            {
                if ((HierarchyProNotesLibrary.instance == null) || (HierarchyProNotesLibrary.instance.gameObject == null))
                {
                    HierarchyProNotesLibrary.FindInstance();
                }
                return HierarchyProNotesLibrary.instance;
            }
        }

        public static IEnumerable<HierarchyProNoteTransform> TransformNotes { get { return HierarchyProNotesLibrary.Instance.transformNotes ?? (HierarchyProNotesLibrary.Instance.transformNotes = new HierarchyProNoteTransform[0]); } }

        public static IHierarchyProNote Create(Transform transform)
        {
            HierarchyProNoteTransform existingNote = HierarchyProNotesLibrary.TransformNotes.FirstOrDefault(x => x.Transform == transform);
            if (existingNote != null)
            {
                return existingNote;
            }

            HierarchyProNoteTransform note = new HierarchyProNoteTransform(transform);
            List<HierarchyProNoteTransform> notes = HierarchyProNotesLibrary.TransformNotes.ToList();
            notes.Add(note);
            HierarchyProNotesLibrary.Instance.transformNotes = notes.ToArray();
            return note;
        }

        public static IHierarchyProNote Create(HierarchyProGroup group)
        {
            HierarchyProNoteGroup existingNote = HierarchyProNotesLibrary.GroupNotes.FirstOrDefault(x => x.Group == group);
            if (existingNote != null)
            {
                return existingNote;
            }

            HierarchyProNoteGroup note = new HierarchyProNoteGroup(group);
            List<HierarchyProNoteGroup> notes = HierarchyProNotesLibrary.GroupNotes.ToList();
            notes.Add(note);
            HierarchyProNotesLibrary.Instance.groupNotes = notes.ToArray();
            return note;
        }

        public static void Delete(IHierarchyProNote note)
        {
            HierarchyProNoteTransform transformNote = note as HierarchyProNoteTransform;
            if (transformNote != null)
            {
                List<HierarchyProNoteTransform> transformNotes = HierarchyProNotesLibrary.TransformNotes.ToList();
                transformNotes.Remove(transformNote);
                HierarchyProNotesLibrary.Instance.transformNotes = transformNotes.ToArray();
                return;
            }

            HierarchyProNoteGroup groupNote = note as HierarchyProNoteGroup;
            if (groupNote != null)
            {
                List<HierarchyProNoteGroup> groupNotes = HierarchyProNotesLibrary.GroupNotes.ToList();
                groupNotes.Remove(groupNote);
                HierarchyProNotesLibrary.Instance.groupNotes = groupNotes.ToArray();
                return;
            }

            Debug.Log("[HierarchyPro] Could not locate note in library.");
        }

        public static IHierarchyProNote Find(Transform transform)
        {
            return HierarchyProNotesLibrary.TransformNotes.FirstOrDefault(x => x.Transform == transform);
        }

        public static IHierarchyProNote Find(GameObject gameObject)
        {
            return HierarchyProNotesLibrary.TransformNotes.FirstOrDefault(x => x.Transform == gameObject.transform);
        }

        public static IHierarchyProNote Find(HierarchyProGroup group)
        {
            return HierarchyProNotesLibrary.GroupNotes.FirstOrDefault(x => x.Group == group);
        }

        public static HierarchyProNotesLibrary FindInstance()
        {
            if ((HierarchyProNotesLibrary.instance != null) && (HierarchyProNotesLibrary.instance.gameObject != null))
            {
                return HierarchyProNotesLibrary.instance;
            }

            HierarchyProNotesLibrary.instance = Object.FindObjectOfType<HierarchyProNotesLibrary>();
            if ((HierarchyProNotesLibrary.instance != null) && (HierarchyProNotesLibrary.instance.gameObject != null))
            {
                return HierarchyProNotesLibrary.instance;
            }

            GameObject gameObject = GameObject.Find("HierarchyPro Data") ?? new GameObject("HierarchyPro Data");
            gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy;
            HierarchyProNotesLibrary library = gameObject.AddComponent<HierarchyProNotesLibrary>();
            HierarchyProNotesLibrary.instance = library;
            return HierarchyProNotesLibrary.instance;
        }
    }
}
