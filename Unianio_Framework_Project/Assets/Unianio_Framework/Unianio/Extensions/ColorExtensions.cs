using UnityEngine;

namespace Unianio.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithA(this Color c, double a)
        {
            return new Color(c.r,c.g,c.b,(float)a);
        }
        public static Color WithR(this Color c, double r)
        {
            return new Color((float)r,c.g,c.b,c.a);
        }
        public static Color WithG(this Color c, double g)
        {
            return new Color(c.r,(float)g,c.b,c.a);
        }
        public static Color WithB(this Color c, double b)
        {
            return new Color(c.r,c.g,(float)b,c.a);
        }
    }
}