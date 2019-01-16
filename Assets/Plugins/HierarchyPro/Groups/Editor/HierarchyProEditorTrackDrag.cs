namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEngine;

    public class HierarchyProEditorTrackDrag
    {
        private const float MinDragDist = 5;

        private Vector2 mousePressPosition;
        private bool isDragging;

        public event Action<Vector2> Drag;
        public event Action<Vector2, Vector2> Dragging;
        public event Action<Vector2, Vector2> Drop;

        public void Update(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    this.mousePressPosition = e.mousePosition;
                    break;

                case EventType.MouseUp:
                    if (this.isDragging)
                    {
                        this.OnDrop(this.mousePressPosition, e.mousePosition);
                    }

                    this.isDragging = false;
                    break;

                case EventType.MouseDrag:
                    if (this.isDragging)
                    {
                        this.OnDragging(this.mousePressPosition, e.mousePosition);
                    }
                    if (!this.isDragging && (Vector2.Distance(this.mousePressPosition, e.mousePosition) >= HierarchyProEditorTrackDrag.MinDragDist))
                    {
                        this.isDragging = true;
                        this.OnDrag(this.mousePressPosition);
                    }
                    break;
            }
        }

        private void OnDrag(Vector2 start)
        {
            if (this.Drag == null)
            {
                return;
            }

            try
            {
                this.Drag(start);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void OnDragging(Vector2 start, Vector2 current)
        {
            if (this.Dragging == null)
            {
                return;
            }

            try
            {
                this.Dragging(start, current);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void OnDrop(Vector2 start, Vector2 drop)
        {
            if (this.Drop == null)
            {
                return;
            }

            try
            {
                this.Drop(start, drop);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
