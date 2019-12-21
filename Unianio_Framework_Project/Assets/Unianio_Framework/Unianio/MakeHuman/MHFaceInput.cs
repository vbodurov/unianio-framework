using UnityEngine;

namespace Unianio.MakeHuman
{
    public class MHFaceInput
    {
        public MHFaceInput(
            Transform eyeL, Transform eyeR,
            Transform jaw,
            Transform eyelidUpL, Transform eyelidDnL,
            Transform eyelidUpR, Transform eyelidDnR,
            Transform special04,
            Transform oris04_L,
            Transform oris03_L,
            Transform oris04_R,
            Transform oris03_R,
            Transform oris01,
            Transform oris02,
            Transform special01,
            Transform oris05,
            Transform oris06,
            Transform oris06_L,
            Transform oris07_L,
            Transform oris06_R,
            Transform oris07_R,
            Transform levator02_L,
            Transform levator03_L,
            Transform levator04_L,
            Transform levator05_L,
            Transform levator02_R,
            Transform levator03_R,
            Transform levator04_R,
            Transform levator05_R,
            Transform temporalis02_L,
            Transform risorius02_L,
            Transform risorius03_L,
            Transform temporalis02_R,
            Transform risorius02_R,
            Transform risorius03_R,
            Transform temporalis01_R,
            Transform oculi02_R,
            Transform oculi01_R,
            Transform temporalis01_L,
            Transform oculi02_L,
            Transform oculi01_L
            )
        {
            EyeL = eyeL;
            EyeR = eyeR;
            EyelidUpL = eyelidUpL;
            EyelidDnL = eyelidDnL;
            EyelidUpR = eyelidUpR;
            EyelidDnR = eyelidDnR;
            Jaw = jaw;
            Special04 = special04;
            Oris04L = oris04_L;
            Oris03L = oris03_L;
            Oris04R = oris04_R;
            Oris03R = oris03_R;
            Oris01 = oris01;
            Oris02 = oris02;
            Special01 = special01;
            Oris05 = oris05;
            Oris06 = oris06;
            Oris06L = oris06_L;
            Oris07L = oris07_L;
            Oris06R = oris06_R;
            Oris07R = oris07_R;
            Levator02L = levator02_L;
            Levator03L = levator03_L;
            Levator04L = levator04_L;
            Levator05L = levator05_L;
            Levator02R = levator02_R;
            Levator03R = levator03_R;
            Levator04R = levator04_R;
            Levator05R = levator05_R;
            Temporalis02L = temporalis02_L;
            Risorius02L = risorius02_L;
            Risorius03L = risorius03_L;
            Temporalis02R = temporalis02_R;
            Risorius02R = risorius02_R;
            Risorius03R = risorius03_R;
            Temporalis01R = temporalis01_R;
            Oculi02R = oculi02_R;
            Oculi01R = oculi01_R;
            Temporalis01L = temporalis01_L;
            Oculi02L = oculi02_L;
            Oculi01L = oculi01_L;

        }
        public readonly Transform EyeL, EyeR, Jaw, 
            EyelidUpL, EyelidDnL, EyelidUpR, EyelidDnR,
            Special04, Oris04L, Oris03L, Oris04R, Oris03R, Oris01, Oris02,
            Special01, Oris05, Oris06, Oris06L, Oris07L, Oris06R, Oris07R,
            Levator02L, Levator03L, Levator04L, Levator05L, Levator02R,
            Levator03R, Levator04R, Levator05R, Temporalis02L, Risorius02L,
            Risorius03L, Temporalis02R, Risorius02R, Risorius03R, Temporalis01R,
            Oculi02R, Oculi01R, Temporalis01L, Oculi02L, Oculi01L
            ;
    }
}