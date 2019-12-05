using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class color
        {
            public static Color Parse(uint color)
            {
                byte r = (byte)(color >> 24);
                byte g = (byte)(color >> 16);
                byte b = (byte)(color >> 8);
                byte a = (byte)(color >> 0);
                return new Color(r / (float)0xFF, g / (float)0xFF, b / (float)0xFF, a / (float)0xFF);
            }
            public static uint ToUInt(Color color)
            {
                byte r = (byte)(color.r * 0xFF);
                byte g = (byte)(color.g * 0xFF);
                byte b = (byte)(color.b * 0xFF);
                byte a = (byte)(color.a * 0xFF);

                return (uint)((r << 24) | (g << 16) | (b << 8) | (a << 0));
            }
            public static Color Rainbow(double x01)
            {
                return FromHueSaturationLuminance(x01.Clamp01().From01ToRange(0, 0.9999), 1, 0.5);
            }
            public static Color FromHueSaturationLuminance(double hue, double saturation, double luminance)
            {
                float v;
                float r, g, b;
                // default to gray
                r = (float)luminance;
                g = (float)luminance;
                b = (float)luminance;
                v = (float)((luminance <= 0.5) ? (luminance * (1.0 + saturation)) : (luminance + saturation - luminance * saturation));
                if (v > 0)
                {
                    float m;
                    float sv;
                    int sextant;
                    float fract, vsf, mid1, mid2;
                    m = (float)luminance + (float)luminance - v;
                    sv = (v - m) / v;
                    hue *= 6.0f;
                    sextant = (int)hue;
                    fract = (float)hue - sextant;
                    vsf = v * sv * fract;
                    mid1 = m + vsf;
                    mid2 = v - vsf;
                    switch (sextant)
                    {
                        case 0:
                            r = v;
                            g = mid1;
                            b = m;
                            break;
                        case 1:
                            r = mid2;
                            g = v;
                            b = m;
                            break;
                        case 2:
                            r = m;
                            g = v;
                            b = mid1;
                            break;
                        case 3:
                            r = m;
                            g = mid2;
                            b = v;
                            break;
                        case 4:
                            r = mid1;
                            g = m;
                            b = v;
                            break;
                        case 5:
                            r = v;
                            g = m;
                            b = mid2;
                            break;
                    }
                }
                return new Color(r, g, b);
            }
        }
    }
}