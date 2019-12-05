namespace Unianio.Animations.Common
{
    public class NextFrameAni : StateHolderAni
    {
        int _frame = 0;
        public override void Update()
        {
            if (_frame > 0)
            {
                Finish();
            }
            ++_frame;
        }
    }
}