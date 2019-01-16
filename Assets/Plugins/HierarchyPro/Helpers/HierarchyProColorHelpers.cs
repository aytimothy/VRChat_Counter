namespace UntitledGames.Hierarchy
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class HierarchyProColorHelpers
    {
        public static Color HSVToRGB(float h, float s, float v)
        {
            Color result = Color.white;
            if (s <= 0.0)
            {
                result.r = v;
                result.g = v;
                result.b = v;
            }
            else if (v <= 0.0)
            {
                result.r = 0.0f;
                result.g = 0.0f;
                result.b = 0.0f;
            }
            else
            {
                result.r = 0.0f;
                result.g = 0.0f;
                result.b = 0.0f;
                float num1 = s;
                float num2 = v;
                float f = h * 6f;
                int num3 = (int) Mathf.Floor(f);
                float num4 = f - num3;
                float num5 = num2 * (1f - num1);
                float num6 = num2 * (float) (1.0 - (num1 * (double) num4));
                float num7 = num2 * (float) (1.0 - (num1 * (1.0 - num4)));
                switch (num3 + 1)
                {
                    case 0:
                        result.r = num2;
                        result.g = num5;
                        result.b = num6;
                        break;
                    case 1:
                        result.r = num2;
                        result.g = num7;
                        result.b = num5;
                        break;
                    case 2:
                        result.r = num6;
                        result.g = num2;
                        result.b = num5;
                        break;
                    case 3:
                        result.r = num5;
                        result.g = num2;
                        result.b = num7;
                        break;
                    case 4:
                        result.r = num5;
                        result.g = num6;
                        result.b = num2;
                        break;
                    case 5:
                        result.r = num7;
                        result.g = num5;
                        result.b = num2;
                        break;
                    case 6:
                        result.r = num2;
                        result.g = num5;
                        result.b = num6;
                        break;
                    case 7:
                        result.r = num2;
                        result.g = num7;
                        result.b = num5;
                        break;
                }

                result.r = Mathf.Clamp(result.r, 0.0f, 1f);
                result.g = Mathf.Clamp(result.g, 0.0f, 1f);
                result.b = Mathf.Clamp(result.b, 0.0f, 1f);
            }
            return result;
        }

        public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
        {
            if ((rgbColor.b > (double) rgbColor.g) && (rgbColor.b > (double) rgbColor.r))
            {
                HierarchyProColorHelpers.RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
            }
            else if (rgbColor.g > (double) rgbColor.r)
            {
                HierarchyProColorHelpers.RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
            }
            else
            {
                HierarchyProColorHelpers.RGBToHSVHelper(0.0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
            }
        }

        private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float h, out float s, out float v)
        {
            v = dominantcolor;
            if (!Mathf.Approximately(v, 0.0f))
            {
                float num1 = (double) colorone <= (double) colortwo ? colorone : colortwo;
                float num2 = v - num1;
                if (!Mathf.Approximately(num2, 0.0f))
                {
                    s = num2 / v;
                    h = offset + ((colorone - colortwo) / num2);
                }
                else
                {
                    s = 0.0f;
                    h = offset + (colorone - colortwo);
                }
                h = h / 6f;
                if (h >= 0.0)
                {
                    return;
                }
                h = h + 1f;
            }
            else
            {
                s = 0.0f;
                h = 0.0f;
            }
        }
    }
}
