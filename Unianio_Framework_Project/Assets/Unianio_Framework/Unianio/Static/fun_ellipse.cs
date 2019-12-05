using System;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class ellipse
        {
            /// <summary>
            /// 0 degrees is horizontal, 90 degrees is vertical
            /// RadiusByAngle(10,20,0) == 10
            /// RadiusByAngle(10,20,90) == 20
            /// alse
            /// RadiusByAngle(200, 50, 20)==RadiusByAngle(200, 50, -20)==RadiusByAngle(200, 50, 180-20)==RadiusByAngle(200, 50, 180+20)==120.5023                       
            /// </summary>
            public static float RadiusByAngle(double horzRadius, double vertRadius, double degrees)
            {
                var angleRadians = DTR * degrees;
                return (float)((horzRadius * vertRadius) / Math.Sqrt(horzRadius * horzRadius * Math.Pow(Math.Sin(angleRadians), 2) + vertRadius * vertRadius * Math.Pow(Math.Cos(angleRadians), 2)));
            }
        }

    }
}