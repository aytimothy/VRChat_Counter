namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditorNoteWindow : PopupWindowContent
    {
        private static readonly Color[] colors;
        private static readonly Texture[] icons;
        private readonly IHierarchyProNote note;

        static HierarchyProEditorNoteWindow()
        {
            HierarchyProEditorNoteWindow.icons = new[]
                                                 {
                                                     HierarchyProEditorIcons.Note,
                                                     HierarchyProEditorIcons.Info,
                                                     HierarchyProEditorIcons.Warning,
                                                     HierarchyProEditorIcons.Error,
                                                     HierarchyProEditorIcons.Favorite
                                                 };

            HierarchyProEditorNoteWindow.colors = new[]
                                                  {
                                                      HierarchyProEditorNoteWindow.ConvertRGB(26, 188, 156),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(46, 204, 113),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(52, 152, 219),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(155, 89, 182),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(52, 73, 94),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(241, 196, 15),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(230, 126, 34),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(231, 76, 60),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(149, 165, 166),
                                                      Color.white,
                                                      HierarchyProEditorNoteWindow.ConvertRGB(22, 160, 133),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(39, 174, 96),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(41, 128, 185),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(142, 68, 173),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(44, 62, 80),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(243, 156, 18),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(211, 84, 0),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(192, 57, 43),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(127, 140, 141),
                                                      HierarchyProEditorNoteWindow.ConvertRGB(189, 195, 199)
                                                  };
        }

        public HierarchyProEditorNoteWindow(IHierarchyProNote note)
        {
            this.note = note;
        }

        public static IEnumerable<Texture> Icons { get { return HierarchyProEditorNoteWindow.icons; } }

        private static Color ConvertRGB(byte r, byte g, byte b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        /// <inheritdoc />
        public override void OnGUI(Rect rect)
        {
            EditorGUI.DrawRect(rect, this.note.ColorBackground);

            Rect headerRect = new Rect(rect) {height = 24};
            EditorGUI.DrawRect(headerRect, Color.white);
            this.DrawHeader(headerRect);

            Rect bodyRect = new Rect(rect) {y = rect.y + headerRect.height, height = rect.height - headerRect.height};

            Color textColor = this.note.ColorBackground.HighContrast();
            Color caretColorReset = GUI.skin.settings.cursorColor;
            GUI.contentColor = textColor;
            GUI.skin.settings.cursorColor = textColor;
            this.note.Text = EditorGUI.TextArea(bodyRect, this.note.Text, HierarchyProEditorStyles.RichTextArea);
            GUI.skin.settings.cursorColor = caretColorReset;
        }

        private void DrawColorPicker(Rect rect)
        {
            int rows = 2;
            int columns = 10;
            float width = rect.width / columns;
            float height = rect.height / rows;

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Rect colorRect = new Rect(rect) {x = rect.x + (width * x), y = rect.y + (height * y), width = width, height = height};
                    int index = x + (y * columns);
                    Color color = HierarchyProEditorNoteWindow.colors[index];
                    EditorGUI.DrawRect(colorRect, color);
                    if (GUI.Button(colorRect, GUIContent.none, GUIStyle.none))
                    {
                        this.note.ColorBackground = color;
                        EditorApplication.RepaintHierarchyWindow();
                        HierarchyProEditorGroupWindow.Redraw();
                    }
                }
            }
        }

        private void DrawHeader(Rect rect)
        {
            Rect iconRect = new Rect(rect) {x = rect.x + 5, width = 16};

            foreach (Texture icon in HierarchyProEditorNoteWindow.Icons)
            {
                if (this.note.Icon == icon)
                {
                    EditorGUI.DrawRect(iconRect, GUI.skin.settings.selectionColor);
                }

                if (icon != null)
                {
                    GUI.DrawTexture(iconRect.GetCenteredIconRect(icon, 16, 16), icon);
                }

                if (GUI.Button(iconRect, GUIContent.none, GUIStyle.none))
                {
                    this.note.Icon = icon;
                    EditorApplication.RepaintHierarchyWindow();
                    HierarchyProEditorGroupWindow.Redraw();
                }

                iconRect = new Rect(iconRect) {x = iconRect.x + 16};
            }

            float x = iconRect.x + 5;
            Rect colorFore = new Rect(rect) {x = x, width = rect.width - x - 26};
            this.DrawColorPicker(colorFore);

            Rect rectDelete = new Rect(colorFore) {x = colorFore.x + colorFore.width + 5, width = 16};
            GUI.color = new Color(0.3f, 0.3f, 0.3f);
            GUI.DrawTexture(rectDelete.GetCenteredIconRect(HierarchyProEditorIcons.Delete), HierarchyProEditorIcons.Delete);
            GUI.color = Color.white;
            if (GUI.Button(rectDelete, GUIContent.none, GUIStyle.none))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to delete this note?", "Yes", "No"))
                {
                    this.editorWindow.Close();
                    HierarchyProNotesLibrary.Delete(this.note);
                    EditorApplication.RepaintHierarchyWindow();
                    HierarchyProEditorGroupWindow.Redraw();
                }
            }
        }
    }
}
