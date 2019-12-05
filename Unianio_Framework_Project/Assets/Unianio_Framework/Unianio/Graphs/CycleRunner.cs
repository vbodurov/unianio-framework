using System;
using static Unianio.Static.fun;

namespace Unianio.Graphs
{
    /*
     var cr = new CycleRunner();

     cr
     .SetCondition(cr=> cr.ExternalCycleIndex >= 0)
     .SetInit(cr => { ... })
     .SetUpdate((cr,x)=> { ... })
     ;

     StartFuncAni(3, x => {
        cr.Run(0.2, 1, x);
     });
     */
    public sealed class CycleRunner : BaseCycleRunner<CycleRunner>
    {
        public CycleRunner(string label = "") { Label = label; }
        public float Run(double start, double externalX01) => Run(start, 1, externalX01);
        public float Run(double start, int divider, double externalX01)
        {
            if (start < 0.0) throw new ArgumentException("CycleRunner start cannot be less than 0");
            if (divider < 1) divider = 1;
            if (externalX01 > 1.0) externalX01 = externalX01 % 1.0;

            SetExternal(externalX01);

            var x = externalX01 - start;
            if (abs(x) > 1.0) x = x % 1.0;
            if (x < 0) x = 1.0 + x;
            if (x <= 0.999 || x >= 1.001)
                x = (x % (1f / (float)divider)) * divider;

            return ApplyInternal((float)x);
        }
    }
}