using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Events;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Services
{
    [AsSingleton]
    public interface IHumanManager : IAnimation
    {
        IComplexHuman GetHumanByPersona(string persona);
    }
    public class HumanManager : AnimationBase, IHumanManager
    {
        readonly LinkedList<IComplexHuman> _list = new LinkedList<IComplexHuman>();
        readonly Dictionary<string, LinkedListNode<IComplexHuman>> _dict = new Dictionary<string, LinkedListNode<IComplexHuman>>();

        public override void Initialize()
        {
            subscribe<HumanCreated>(OnHumanCreated, this);
            subscribe<HumanDestroyed>(OnHumanDestroyed, this);
        }
        public override void Update()
        {
            var node = _list.First;
            while (node != null)
            {
                var human = node.Value;
                if (!human.IsDisabled)
                {
                    human.LateUpdate();
                }
                node = node.Next;
            }
        }
        IComplexHuman IHumanManager.GetHumanByPersona(string persona)
        {
            return _dict.TryGetValue(persona, out var node) ? node.Value : null;
        }
        void OnHumanCreated(ComplexHumanGlobalEvent e)
        {
            OnHumanDestroyed(e);
            _dict[e.Human.Persona] = _list.AddLast(e.Human);
            fire(new HumanRegistered(e.Human));
        }
        void OnHumanDestroyed(ComplexHumanGlobalEvent e)
        {
            if (_dict.TryGetValue(e.Human.Persona, out var node))
            {
                _dict.Remove(e.Human.Persona);
                _list.Remove(node);
                e.Human.Dispose();
                fire(new HumanUnregistered(e.Human));
            }
        }
    }
}