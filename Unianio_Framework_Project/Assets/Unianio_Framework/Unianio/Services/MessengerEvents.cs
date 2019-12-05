using System;
using Unianio.Animations;
//using Unianio.Enums;
using UnityEngine;

namespace Unianio.Services
{
    public abstract class NamedEvent :  BaseEvent
    {
        protected NamedEvent(object name)
        {
            Name = Convert.ToString(name);
        }
        public string Name { get; }
        public int Num { get; set; } // to be used for tests
    }
    public interface IUniqueEvent { }
    public class EnsureNamedUniqueEvent : NamedEvent, IUniqueEvent
    {
        public readonly object Requester;
        public EnsureNamedUniqueEvent(string name, object requester) : base(name)
        {
            Requester = requester;
        }
    }
    public class PlayAni : BaseEvent { public IAnimation Ani; public int QueueIndex = 0; }
//    public class InputControllerIsOn : BaseEvent { public ControllerSide Side; public GameObject GameObject; internal Transform[] Models; }
}