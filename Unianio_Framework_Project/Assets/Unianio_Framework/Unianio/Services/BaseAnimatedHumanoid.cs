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

        IAnimation AniByType(BodyPart p);
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

        IAnimation IAnimatedHumanoid.AniByType(BodyPart p)
        {
            switch (p)
            {
                case BodyPart.EntireBody: return _this.AniEntireBody;
                case BodyPart.Face: return _this.AniFace;
                case BodyPart.Head: return _this.AniHead;
                case BodyPart.ArmR: return _this.AniArmR;
                case BodyPart.ArmL: return _this.AniArmL;
                case BodyPart.LegR: return _this.AniLegR;
                case BodyPart.LegL: return _this.AniLegL;
                case BodyPart.HandL: return _this.AniHandL;
                case BodyPart.HandR: return _this.AniHandR;
                case BodyPart.Torso: return _this.AniTorso;
                case BodyPart.BreastL: return _this.AniBreastL;
                case BodyPart.BreastR: return _this.AniBreastR;
                case BodyPart.Spine: return _this.AniSpine;
                case BodyPart.BothEyelids: return _this.AniBlink;
                case BodyPart.Jaw: return _this.AniJaw;
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