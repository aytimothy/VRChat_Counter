namespace UntitledGames.Hierarchy
{
    using UnityEngine;

    public interface IHierarchyProNote
    {
        Color ColorBackground { get; set; }
        Texture Icon { get; set; }
        string Text { get; set; }
    }
}
