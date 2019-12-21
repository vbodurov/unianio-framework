using System;

namespace Unianio.Genesis.State
{
    /// <summary>
    /// open:
    /// EyesUp = 0,EyesLo = 0
    /// 
    /// close:
    /// EyesUp = 1,EyesLo = 1
    /// 
    /// happy:
    /// EyesUp = -0.4, EyesLo = 1.0
    ///
    /// don't care:
    /// EyesUp = 0.5,EyesLo = -0.8
    /// </summary>
    public class SetGenesisFace
    {
        public double? EyebrowsInner { get; set; }
        public double? EyebrowsMiddle { get; set; }
        public double? EyebrowsOuter { get; set; }
        public double? Nose { get; set; }
        public double? EyesUp { get; set; }
        public double? EyesLo { get; set; }
        public double? Cheeks { get; set; }
        public double? MouthStretch { get; set; }
        public double? MouthSmile { get; set; }
        public double? MouthSides { get; set; }
        public double? MouthCenter { get; set; }
        public double? Jaw { get; set; }
        public double? Tongue { get; set; }
        public Func<double, double> Func { get; set; }
        public Func<double, double> FuncNose { get; set; }
        public Func<double, double> FuncJaw { get; set; }
        public Func<double, double> FuncTongue { get; set; }

    }
}