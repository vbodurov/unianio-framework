using System;

namespace Unianio.Graphs
{
    /*
     var ir = new IntervalRunner();

     ir
     .SetCondition(cr=> cr.ExternalCycleIndex >= 0)
     .SetInit(cr => { ... })
     .SetUpdate((cr,x)=> { ... })
     ;

     StartFuncAni(3, x => {
        ir.RunBetween(0.2, 0.5, x).And.RunBetween(0.7, 0.9, x);
     });
     */
    public sealed class IntervalRunner : BaseCycleRunner<IntervalRunner>
    {

        public IntervalRunner And => this;
        public IntervalRunner(string label = "") { Label = label; }
        public float RunBetween(double from, double to, double externalX01)
        {
            if (from < 0 || from > 1) throw new ArgumentException("IntervalRunner from variable must be between 0 and 1, it is " + from);
            if (to < 0 || to > 1) throw new ArgumentException("IntervalRunner to variable must be between 0 and 1, it is " + to);

            SetExternal(externalX01);

            double internalX01;
            if (from > to)
            {
                var range = (1.0 - from) + to;
                if (ExternalX01 >= from)
                {
                    internalX01 = (ExternalX01 - from) / range;
                }
                else if (ExternalX01 <= to)
                {
                    internalX01 = ((1.0 - from) + ExternalX01) / range;
                }
                else
                {
                    return OutOfRange();
                }
            }
            else
            {
                if (ExternalX01 < from || ExternalX01 > to)
                {
                    return OutOfRange();
                }

                var curr = ExternalX01 - from;
                var range = to - from;
                internalX01 = curr / range;
            }


            return ApplyInternal(internalX01);
        }
    }
}