
using Unianio.Moves;

namespace Unianio.Human
{
    public interface IBlinkController
    {
        /// <summary>
        /// 0 = open eyes
        /// 1 = closed eyes
        /// </summary>
        double Blink01 { get; set; }
        double BlinkUpL01 { get; set; }
        double BlinkUpR01 { get; set; }
        double BlinkDnL01 { get; set; }
        double BlinkDnR01 { get; set; }
        /// <summary>
        /// 1 = ark up like in smile
        /// 0 = normal
        /// -1 = ark down like when looking down
        /// </summary>
        double EyeCurveShiftM11 { get; set; }
        NumericMover MoveEyeCurveShift { get; }
        NumericMover MoveBlink { get; }
        NumericMover MoveBlinkUpL { get; }
        NumericMover MoveBlinkUpR { get; }
        NumericMover MoveBlinkDnL { get; }
        NumericMover MoveBlinkDnR { get; }
        
    }
}