using System;
using System.Collections;
using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Genesis;
using Unianio.IK;
using Unianio.MakeHuman;
using Unianio.PhysicsAgents;
using Unianio.Rigged;
//TODO:enable
//using Unianio.Genesis;
//using Unianio.IK;
//using Unianio.PhysicsAgents;
//using Unianio.Rigged;
using Unianio.Services;
using UnityEngine;

namespace Unianio.Events
{
    public class PlayerInputsInitialized : BaseEvent
    {
        public Transform PlayArea { get; }
        public Transform CameraFloorOffset { get; }
        public Transform MainCameraHolder { get; }
        public Transform Head { get; }
        public Transform LtHand { get; }
        public Transform RtHand { get; }
        public Transform Tracker1 { get; }
        public Transform Tracker2 { get; }
        public Transform Tracker3 { get; }
        public PlayerInputsInitialized(
            Transform playArea, Transform cameraFloorOffset, Transform mainCameraHolder,
            Transform head, Transform ltHand, Transform rtHand,
            Transform tracker1, Transform tracker2, Transform tracker3)
        {
            PlayArea = playArea;
            CameraFloorOffset = cameraFloorOffset;
            MainCameraHolder = mainCameraHolder;
            Head = head;
            LtHand = ltHand;
            RtHand = rtHand;
            Tracker1 = tracker1;
            Tracker2 = tracker2;
            Tracker3 = tracker3;
        }
    }

    public class ComplexHumanEnabled : ComplexHumanInstanceEvent
    {
        public ComplexHumanEnabled(IComplexHuman human) : base(human) { }
    }
    public class ComplexHumanDisabled : ComplexHumanInstanceEvent
    {
        public ComplexHumanDisabled(IComplexHuman human) : base(human) { }
    }


    public class BeforeCreatingPlayerHand : BaseEvent
    {
        public BodySide Side { get; }
        public bool Suppress { get; set; }
        public BeforeCreatingPlayerHand(BodySide side)
        {
            Side = side;
        }
    }
    public class PlayerHandInputInitialized : BaseEvent
    {
        public Transform Hand { get; }
        public BodySide Side { get; }
        public PlayerHandInputInitialized(Transform hand, BodySide side)
        {
            Hand = hand;
            Side = side;
        }
    }
    public class HapticPulse : BaseEvent
    {
        public BodySide Side { get; }
        public double Amplitude { get; private set; }
        public double Duration { get; private set; }
        public HapticPulse(BodySide side, double amplitude = 0.5, double duration = 1.0)
        {
            Side = side;
            Amplitude = amplitude;
            Duration = duration;
        }
        public HapticPulse Set(double amplitude, double duration)
        {
            Amplitude = amplitude;
            Duration = duration;
            return this;
        }
    }
    public class ShowSubtitles : BaseEvent
    {
        public string Clip { get; }
        public ShowSubtitles(string clip) => Clip = clip;
    }
    public class EnableSubtitles : BaseEvent { }
    public class DisableSubtitles : BaseEvent { }
    public class PlayMusic : BaseEvent
    {
        public int Source { get; }
        public AudioClip AudioClip { get; }
        public bool Loop { get; }
        public double StartTime { get; }

        public PlayMusic(int source, AudioClip audioClip, double startTime, bool loop)
        {
            Source = source;
            AudioClip = audioClip;
            Loop = loop;
            StartTime = startTime;
        }
    }
    public class StopMusic : BaseEvent
    {
        public int Source { get; }
        public StopMusic(int source)
        {
            Source = source;
        }
    }
    public class FadeMusicSource : BaseEvent
    {
        public double Seconds { get; }
        public int Source { get; }
        public MusicFadeType FadeType { get; }
        public double MaxVolume { get; }
        public FadeMusicSource(double seconds, int source, MusicFadeType type, double maxVolume = 1.0)
        {
            Seconds = seconds;
            Source = source;
            FadeType = type;
            MaxVolume = maxVolume;
        }
    }
    public class AudioSettingsChanged : BaseEvent { }
    public class SelectUiElement : BaseEvent
    {
        public ulong Id { get; }
        public SelectUiElement(ulong id) => Id = id;
    }
    public class DeselectUiElement : BaseEvent
    {
        public ulong Id { get; }
        public DeselectUiElement(ulong id) => Id = id;
    }
    
    
    public abstract class ComplexHumanInstanceEvent : NamedEvent
    {
        protected ComplexHumanInstanceEvent(IComplexHuman human) : base(human.ID) { Human = human; }
        public IComplexHuman Human { get; }
    }
    
    public class CancelInertia : ComplexHumanInstanceEvent { public CancelInertia(IComplexHuman human) : base(human){} }
    public class RootAnimationChanged : ComplexHumanInstanceEvent
    {
        public IAnimation Previous { get; }
        public IAnimation Next { get; }
        public RootAnimationChanged(IComplexHuman human, IAnimation previous, IAnimation next) : base(human)
        {
            Previous = previous;
            Next = next;
        }
    }
    public abstract class ComplexHumanGlobalEvent : BaseEvent
    {
        protected ComplexHumanGlobalEvent(IComplexHuman human) { Human = human; }
        public IComplexHuman Human { get; }
    }
    internal class HumanCreated : ComplexHumanGlobalEvent { internal HumanCreated(IComplexHuman human) : base(human) { } }
    internal class HumanDestroyed : ComplexHumanGlobalEvent { internal HumanDestroyed(IComplexHuman human) : base(human) { } }
    public class HumanRegistered : ComplexHumanGlobalEvent { public HumanRegistered(IComplexHuman human) : base(human) { } }
    public class HumanUnregistered : ComplexHumanGlobalEvent { public HumanUnregistered(IComplexHuman human) : base(human) { } }

    public class BeforeCreatingHuman : BaseEvent
    {
        public MakeHumanDefinition Definition { get; }
        public string CustomTag { get; }
        public IList<IHumanExtender> Extenders { get; }
        public BeforeCreatingHuman(MakeHumanDefinition mhd, string customTag)
        {
            Definition = mhd;
            CustomTag = customTag;
            Extenders = new List<IHumanExtender>();
        }
    }

    public class RegisterSoftBodyConfig : ComplexHumanInstanceEvent
    {
        public BodySide Side { get; }
        public BodyPart Part { get; }
        public SoftBodyConfig Config { get; }
        public RegisterSoftBodyConfig(IComplexHuman human, BodySide side, BodyPart part, SoftBodyConfig config) 
            : base(human)
        {
            Side = side;
            Part = part;
            Config = config;
        }
    }
    //TODO:enable
    /*
    public class PlayerBlinkAniStarts : BaseEvent { }
    public class PlayerBlinkAniEnds : BaseEvent { }
    public class TeleportRequest : BaseEvent
    {
        public readonly BodySide Side;
        public readonly Vector3 Target;
        public TeleportRequest(BodySide side, Vector3 target)
        {
            Side = side;
            Target = target;
        }
    }
    */
    public class WhenPlayerTeleportEnds : BaseEvent { }
    public class StartBlinkTransition : BaseEvent
    {
        public StartBlinkTransition(double seconds, Action onEnd)
        {
            Seconds = seconds;
            OnEnd = onEnd;
        }
        public readonly double Seconds;
        public readonly Action OnEnd;
    }
    public class BlinkTransitionStateChanged : BaseEvent
    {
        public BlinkTransitionStateChanged(bool isInTransition)
        {
            IsInTransition = isInTransition;
        }
        public bool IsInTransition { get; private set; }   
    }

    public abstract class GameButtonPress : BaseEvent
    {
        public bool IsPressed;
        public Transform Controller;
        public ControllerSide Side;
        public float Force;
    }
    public class GlobalAudioSourceSet : BaseEvent { }
    public class HmdHeadWasSet : BaseEvent { }
    public class HmdPlayAreaWasSet : BaseEvent { }
    public class HmdRightControllerWasSet : BaseEvent { }
    public class HmdLeftControllerWasSet : BaseEvent { }
    public class BtnTriggerPress : GameButtonPress { }
    public class BtnAppMenuPress : GameButtonPress { }
    public class BtnPadPress : GameButtonPress
    {
        public Vector2 Point { get; }
        public BtnPadPress(Vector2 point) => Point = point;
    }
    public class BtnGripPress : GameButtonPress { }
    public class BtnJumpPress : GameButtonPress { }
    public class BtnSquatPress : GameButtonPress { }
    public class BtnOculusTouch : GameButtonPress
    {
        public char Letter { get; }
        public BtnOculusTouch(char letter) => Letter = letter;
    }

    public abstract class DirectionButtonPress : GameButtonPress
    {
        public Direction Direction { get; }
        protected DirectionButtonPress(Direction direction) => Direction = direction;
    }
    public class BtnUpPress : DirectionButtonPress { public BtnUpPress() : base(Direction.Up) { } }
    public class BtnDownPress : DirectionButtonPress { public BtnDownPress() : base(Direction.Down) { } }
    public class BtnLeftPress : DirectionButtonPress { public BtnLeftPress() : base(Direction.Left) { } }
    public class BtnRightPress : DirectionButtonPress { public BtnRightPress() : base(Direction.Right) { } }
    public class BtnSubmitPress : GameButtonPress
    {
        public ulong Id { get; set; }
    }
    public class BtnSubmitUnpress : GameButtonPress
    {
        public ulong Id { get; set; }
    }
    public class BtnCancelPress : GameButtonPress { }
    public class BtnMenuPress : GameButtonPress { }

    //    internal class ReEvaluateAppMenuButtons : BaseEvent { }
    public class EnableUiButton : BaseEvent
    {
        public readonly int ButtonId;
        public readonly bool IsEnabled;
        public EnableUiButton(int buttonId, bool isEnabled)
        {
            ButtonId = buttonId;
            IsEnabled = isEnabled;
        }
    }
    public class ChangeCheckUiButton : BaseEvent
    {
        public readonly int ButtonId;
        public readonly bool IsChecked;
        public ChangeCheckUiButton(int buttonId, bool isChecked)
        {
            ButtonId = buttonId;
            IsChecked = isChecked;
        }
    }
    public class PanelButtonSelectionChanged : BaseEvent
    {
        public PanelButtonSelectionChanged(int id, bool isSelected) { Id = id; IsSelected = isSelected; }
        public readonly int Id;
        public readonly bool IsSelected;
    }
    public class PanelButtonPressed : BaseEvent
    {
        public PanelButtonPressed(int id, ControllerSide side, bool isDisabled) { Id = id; Side = side; IsDisabled = isDisabled; }
        public readonly int Id;
        public readonly ControllerSide Side;
        public readonly bool IsDisabled;
    }
    
    public class ChangeControllersVisibility : BaseEvent
    {
        public readonly bool Visible;
        public readonly bool Transparent;
        public ChangeControllersVisibility(bool visible, bool transparent)
        {
            Visible = visible;
            Transparent = transparent;
        }
    }
    public class StartGame : BaseEvent { }
    public class EndGame : BaseEvent { }
    //TODO:enable
    /*
    public class PlayerArmInitialized : BaseEvent
    {
        public IPlayerArm Arm { get; }
        public PlayerArmInitialized(IPlayerArm arm)
        {
            Arm = arm;
        }
    }*/
}