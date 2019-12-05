namespace Unianio.Animations.Common
{
    public abstract class StateHolderAni : AnimationBase
    {
        public virtual object State { get; set; }

        public override IAnimation ClearAllFollowUpActions()
        {
            State = null;
            return base.ClearAllFollowUpActions();
        }
    }
}