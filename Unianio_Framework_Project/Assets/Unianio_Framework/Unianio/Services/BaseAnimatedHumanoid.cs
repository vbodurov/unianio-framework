using Unianio.Animations;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.Rigged;

namespace Unianio.Services
{
    public interface IAnimatedHumanoid
    {
        IAnimation AniEntireBody { get; set; }
        IAnimation AniHead { get; set; }
        IAnimation AniArmR { get; set; }
        IAnimation AniArmL { get; set; }
        IAnimation AniLegR { get; set; }
        IAnimation AniLegL { get; set; }
        IAnimation AniHandL { get; set; }
        IAnimation AniHandR { get; set; }
        IAnimation AniTorso { get; set; }
        IAnimation AniBreastL { get; set; }
        IAnimation AniBreastR { get; set; }
        IAnimation AniSpine { get; set; }
        IAnimation AniFace { get; set; }
        IAnimation AniLook { get; set; }
        IAnimation AniBlink { get; set; }
        IAnimation AniJaw { get; set; }

        IAnimation AniByType(HumanoidPart p);
        void ForceStopAllAnimations();
    }
    public abstract class BaseAnimatedHumanoid : IAnimatedHumanoid
    {
        readonly IAnimatedHumanoid _this;
        protected BaseAnimatedHumanoid() => _this = this;
        public virtual IAnimation AniEntireBody { get; set; }
        IAnimation IAnimatedHumanoid.AniHead { get; set; }
        IAnimation IAnimatedHumanoid.AniArmR { get; set; }
        IAnimation IAnimatedHumanoid.AniArmL { get; set; }
        IAnimation IAnimatedHumanoid.AniLegR { get; set; }
        IAnimation IAnimatedHumanoid.AniLegL { get; set; }
        IAnimation IAnimatedHumanoid.AniHandL { get; set; }
        IAnimation IAnimatedHumanoid.AniHandR { get; set; }
        IAnimation IAnimatedHumanoid.AniTorso { get; set; }
        IAnimation IAnimatedHumanoid.AniBreastL { get; set; }
        IAnimation IAnimatedHumanoid.AniBreastR { get; set; }
        IAnimation IAnimatedHumanoid.AniSpine { get; set; }
        IAnimation IAnimatedHumanoid.AniFace { get; set; }
        IAnimation IAnimatedHumanoid.AniLook { get; set; }
        IAnimation IAnimatedHumanoid.AniBlink { get; set; }
        IAnimation IAnimatedHumanoid.AniJaw { get; set; }

        IAnimation IAnimatedHumanoid.AniByType(HumanoidPart p)
        {
            switch (p)
            {
                case HumanoidPart.EntireBody: return _this.AniEntireBody;
                case HumanoidPart.Face: return _this.AniFace;
                case HumanoidPart.Head: return _this.AniHead;
                case HumanoidPart.ArmR: return _this.AniArmR;
                case HumanoidPart.ArmL: return _this.AniArmL;
                case HumanoidPart.LegR: return _this.AniLegR;
                case HumanoidPart.LegL: return _this.AniLegL;
                case HumanoidPart.HandL: return _this.AniHandL;
                case HumanoidPart.HandR: return _this.AniHandR;
                case HumanoidPart.Torso: return _this.AniTorso;
                case HumanoidPart.BreastL: return _this.AniBreastL;
                case HumanoidPart.BreastR: return _this.AniBreastR;
                case HumanoidPart.Spine: return _this.AniSpine;
                case HumanoidPart.BothEyelids: return _this.AniBlink;
                case HumanoidPart.Jaw: return _this.AniJaw;
            }
            return null;
        }

        void IAnimatedHumanoid.ForceStopAllAnimations()
        {
            var all = new IAnimation[]
            {
                _this.AniHead, _this.AniArmR, _this.AniArmL,
                _this.AniLegR, _this.AniLegL, _this.AniHandL, _this.AniHandR,
                _this.AniTorso, _this.AniBreastL, _this.AniBreastR,
                _this.AniSpine, _this.AniFace,
                _this.AniLook, _this.AniBlink, _this.AniJaw, _this.AniEntireBody
            };
            foreach (var e in all)
            {
                e.ForceStopIfRunning();
            }
        }
    }
}