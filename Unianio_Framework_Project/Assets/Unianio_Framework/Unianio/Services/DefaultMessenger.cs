using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Services
{
    public interface ISubscription
    {
        int ID { get; }
        string Name { get; }
        object Subscriber { get; }
    }
    public abstract class BaseEvent
    {
        internal object Subscriber { get; set; }
    }
//    public abstract class EventWithSender : BaseEvent
//    {
//        internal object Sender { get; set; }
//    }
    public interface ISubscription<T> : ISubscription where T : BaseEvent
    {
        Action<T> Action { get; }
    }
    public class Subscription<T> : ISubscription<T> where T : BaseEvent
    {
        readonly object _subscriber;
        readonly Action<T> _action;
        readonly int _id;
        readonly string _name;
        public Subscription(object subscriber, string name, Action<T> action)
        {
            _subscriber = subscriber;
            _name = name;
            _action = action;
            _id = ++DefaultMessenger.SubscriptionLastId;
        }
        Action<T> ISubscription<T>.Action => _action;
        object ISubscription.Subscriber => _subscriber;
        string ISubscription.Name => _name;
        int ISubscription.ID => _id;
    }
    public struct EventId : IEquatable<EventId>
    {
        readonly int _hashCode;
        readonly Type _type;
        readonly string _name;
        public EventId(Type type)
        {
            _type = type;
            _name = null;
            _hashCode = type.GetHashCode();
        }
        public EventId(Type type, string name)
        {
            _type = type;
            _name = name;
            var hash = type.GetHashCode();
            if (name != null)
            {
                unchecked
                {
                    hash = (31 * hash) ^ name.GetHashCode();
                }     
            }      
            _hashCode = hash;
        }
        public Type Type => _type;
        public string Name => _name;
        public bool Equals(EventId other)
        {
            return other.Type == _type && other.Name == _name;
        }
        public override int GetHashCode() { return _hashCode; }
        public override string ToString() { return _name == null ? _type.ToString() : _type +"_"+_name; }
    }
    public class EventEqualityComparer : IEqualityComparer<EventId>
    {
        bool IEqualityComparer<EventId>.Equals(EventId x, EventId y)
        {
            return x.Equals(y);
        }

        int IEqualityComparer<EventId>.GetHashCode(EventId e)
        {
            return e.GetHashCode();
        }
    }


    public interface IMessenger
    {
        void Subscribe<T>(object subscriber, Action<T> action) where T : BaseEvent;
        void Subscribe<T>(object subscriber, string name, Action<T> action) where T : BaseEvent;
        //void SubscribeOrInvoke<T>(object subscriber, bool condition, Action<T> action) where T : BaseEvent;
        //void SubscribeOrInvoke<T>(object subscriber, string name, bool condition, Action<T> action) where T : BaseEvent;
        //void InvokeAndSubscribe<T>(object subscriber, bool condition, Action<T> action) where T : BaseEvent;
        //void InvokeAndSubscribe<T>(object subscriber, string name, bool condition, Action<T> action) where T : BaseEvent;
        int Invoke<T>(T arg) where T : BaseEvent;
        int Invoke<T>() where T : BaseEvent, new();
        int InvokeOrQueue<T>(T arg) where T : BaseEvent;
        int InvokeOrQueue<T>() where T : BaseEvent, new();
        int Unsubscribe(object subscriber);
        int UnsubscribeByArg<T>(object subscriber) where T : BaseEvent;
        int UnsubscribeByArg<T>() where T : BaseEvent;
        int UnsubscribeByArg<T>(object subscriber,string name) where T : BaseEvent;
        int UnsubscribeByArg<T>(string name) where T : BaseEvent;
        void ClearAll();
        string Map { get; }
        int UnsubscribeByArgIgnoreName<T>() where T : BaseEvent;
        void ClearEventQueue();
        bool TryClearEventFromQueue<T>(string name = null) where T : BaseEvent;
    }
    public sealed class DefaultMessenger : IMessenger
    {
        internal static int SubscriptionLastId = 0;

        static readonly EventEqualityComparer _comparer = new EventEqualityComparer();

        readonly IDictionary<object, IDictionary<EventId, ISubscription>> _allBySubscriber =
            new Dictionary<object, IDictionary<EventId, ISubscription>>();

        readonly IDictionary<EventId, IDictionary<object, ISubscription>> _allByEventId =
            new Dictionary<EventId, IDictionary<object, ISubscription>>(_comparer);

        readonly IDictionary<EventId, IList<BaseEvent>> _eventQueue = 
            new Dictionary<EventId, IList<BaseEvent>>();

        readonly IMessenger _messenger;

        public DefaultMessenger()
        {
            SubscriptionLastId = 0;
            _messenger = this;
        }
        void InternalSubscribe<T>(ISubscription<T> subscription) where T : BaseEvent
        {
            var eventId = new EventId(typeof (T), subscription.Name);

            // invoke queued events
            IList<BaseEvent> list;
            if(_eventQueue.TryGetValue(eventId, out list))
            {
                foreach (var e in list)
                {
                    subscription.Action((T)e);
                }
                _eventQueue.Remove(eventId);
            }

            IDictionary<EventId, ISubscription> byEventId;
            if (_allBySubscriber.TryGetValue(subscription.Subscriber, out byEventId))
            {
                byEventId[eventId] = subscription;
            }
            else
            {
                _allBySubscriber[subscription.Subscriber] =
                    new Dictionary<EventId, ISubscription>(_comparer) { { eventId, subscription } };
            }

            IDictionary<object, ISubscription> bySubscriber;
            if (_allByEventId.TryGetValue(eventId, out bySubscriber))
            {
                bySubscriber[subscription.Subscriber] = subscription;
            }
            else
            {
                _allByEventId[eventId] =
                    new Dictionary<object, ISubscription> { { subscription.Subscriber, subscription } };
            }
        }

        void IMessenger.ClearEventQueue()
        {
            _eventQueue.Clear();
        }
        bool IMessenger.TryClearEventFromQueue<T>(string name)
        {
            var eventId = new EventId(typeof(T), name);

            return _eventQueue.Remove(eventId);
        }
        void IMessenger.Subscribe<T>(object subscriber, Action<T> action) 
        {
            if (typeof (NamedEvent).IsAssignableFrom(typeof (T)))
            {
                throw new InvalidOperationException("Subscription for named event {0} is not valid because it requires name".Args(typeof(T)));
            }
            InternalSubscribe(new Subscription<T>(subscriber, null, action));
        }
        void IMessenger.Subscribe<T>(object subscriber, string name, Action<T> action)
        {
            InternalSubscribe(new Subscription<T>(subscriber, name, action));
        }
        int IMessenger.Invoke<T>(T arg)
        {
            return InternalInvoke(arg, Int32.MaxValue);
        }
        int IMessenger.InvokeOrQueue<T>()
        {
            return _messenger.InvokeOrQueue(new T());
        }
        int IMessenger.InvokeOrQueue<T>(T evt)
        {
            var i = _messenger.Invoke(evt);
            // nothing was invoked then queue
            if (i == 0)
            {
                var named = evt as NamedEvent;
                var type = typeof(T);
                var eventId = new EventId(type, named?.Name);
                if(!_eventQueue.TryGetValue(eventId, out var list))
                {
                    _eventQueue[eventId] = list = new List<BaseEvent>();
                }
                list.Add(evt);
            }
            return i;
        }

        int IMessenger.Invoke<T>()
        {
            return InternalInvoke(new T(), Int32.MaxValue);
        }

        int InternalInvoke<T>(T arg, int maxNumberInvocations) where T : BaseEvent
        {
            var invokedOn = 0;
            var named = arg as NamedEvent;
            var type = typeof(T);
            var eventId = new EventId(type, named?.Name);
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                foreach (var subscription in bySubscriber.Values.ToArray())
                {
                    if (!(subscription is ISubscription<T> typedSubscription))
                    {
                        throw new ArgumentException(
                                "Subscription of type {0} was invoked as type Subscription`1[{1}]"
                                    .Args(subscription, eventId));
                    }
                    if (invokedOn > maxNumberInvocations)
                    {
                        return invokedOn;
                    }
                    ++invokedOn;
                    arg.Subscriber = typedSubscription.Subscriber;
                    typedSubscription.Action(arg);
                }
            }
            return invokedOn;
        }

        
        int IMessenger.Unsubscribe(object subscriber)
        {
            if (subscriber == null) return 0;

            var count = 0;
            if (_allBySubscriber.TryGetValue(subscriber, out var byType))
            {
                foreach (var kvp in byType)
                {
                    if (!_allByEventId.TryGetValue(kvp.Key, out var bySubscriber))
                    {
                        continue;
                    }
                    if (!bySubscriber.Remove(subscriber))
                    {
                        continue;
                    }
                    ++count;
                    if (bySubscriber.Count == 0)
                    {
                        _allByEventId.Remove(kvp.Key);
                    }
                }
                _allBySubscriber.Remove(subscriber);
            }
            return count;
        }
        int IMessenger.UnsubscribeByArg<T>()
        {
            var count = 0;
            var eventId = new EventId(typeof(T));
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                foreach (var kvp in bySubscriber)
                {
                    if (!_allBySubscriber.TryGetValue(kvp.Key, out var byEventId))
                    {
                        continue;
                    }
                    if (!byEventId.Remove(eventId))
                    {
                        continue;
                    }
                    ++count;
                    if (byEventId.Count == 0)
                    {
                        // if we just removed ny name and the collection became empty - clean up the whole collection
                        _allBySubscriber.Remove(kvp.Key);
                    }
                }
                _allByEventId.Remove(eventId);
            }
            return count;
        }
        int IMessenger.UnsubscribeByArgIgnoreName<T>()
        {
            var count = 0;
            var type = typeof(T);
            var eventId = new EventId(type);
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                foreach (var kvp in bySubscriber)
                {
                    if (!_allBySubscriber.TryGetValue(kvp.Key, out var byEventId))
                    {
                        continue;
                    }
                    if (!byEventId.Remove(eventId))
                    {
                        continue;
                    }
                    ++count;
                    if (byEventId.Count == 0)
                    {
                        // if we just removed ny name and the collection became empty - clean up the whole collection
                        _allBySubscriber.Remove(kvp.Key);
                    }
                }
                _allByEventId.Remove(eventId);
            }

            var remainingNames = _allByEventId.Where(e => e.Key.Type == type && e.Key.Name != null).Select(e => e.Key.Name).ToArray();
            foreach (var name in remainingNames)
            {
                _messenger.UnsubscribeByArg<T>(name);
            }

            return count;
        }

        int IMessenger.UnsubscribeByArg<T>(object subscriber)
        {
            var count = 0;
            var named = subscriber as NamedEvent;
            var eventId = named == null ? new EventId(typeof(T)) : new EventId(typeof(T), named.Name);
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                if (bySubscriber.Remove(subscriber))
                {
                    ++count;
                    if (bySubscriber.Count == 0)
                    {
                        _allByEventId.Remove(eventId);
                    }
                }
            }

            if (_allBySubscriber.TryGetValue(subscriber, out var byEventId))
            {
                if (byEventId.Remove(eventId))
                {
                    if (byEventId.Count == 0)
                    {
                        _allBySubscriber.Remove(subscriber);
                    }
                }
            }
            return count;
        }
        int IMessenger.UnsubscribeByArg<T>(string name)
        {
            var count = 0;
            var eventId = new EventId(typeof(T), name);
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                foreach (var kvp in bySubscriber)
                {
                    if (!_allBySubscriber.TryGetValue(kvp.Key, out var byEventId))
                    {
                        continue;
                    }
                    if (!byEventId.Remove(eventId))
                    {
                        continue;
                    }
                    ++count;
                    if (byEventId.Count == 0)
                    {
                        // if we just removed ny name and the collection became empty - clean up the whole collection
                        _allBySubscriber.Remove(kvp.Key);
                    }
                }
                _allByEventId.Remove(eventId);
            }
            return count;
        }
        int IMessenger.UnsubscribeByArg<T>(object subscriber, string name)
        {
            var count = 0;
            var eventId = new EventId(typeof(T), name);
            if (_allByEventId.TryGetValue(eventId, out var bySubscriber))
            {
                if (bySubscriber.Remove(subscriber))
                {
                    ++count;
                    if (bySubscriber.Count == 0)
                    {
                        _allByEventId.Remove(eventId);
                    }
                }
            }

            if (_allBySubscriber.TryGetValue(subscriber, out var byEventId))
            {
                if (byEventId.Remove(eventId))
                {
                    if (byEventId.Count == 0)
                    {
                        _allBySubscriber.Remove(subscriber);
                    }
                }
            }
            return count;
        }

        
        void IMessenger.ClearAll()
        {
            _allBySubscriber.Clear();
            _allByEventId.Clear();
        }

        string IMessenger.Map
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("{\"by_event_id\":");
                {
                    sb.Append("{");
                    var first = true;
                    foreach (var kvp in _allByEventId)
                    {
                        if (first) first = false;
                        else sb.Append(",");

                        sb.Append("\""+kvp.Key.ToString()+"\":{");

                        var first2 = true;
                        foreach (var kvp2 in kvp.Value)
                        {
                            if (first2) first2 = false;
                            else sb.Append(",");

                            sb.Append("\"s" + kvp2.Key.GetHashCode() + "\":");
                            sb.Append("{\"id\":" + kvp2.Value.ID + ", \"subscriber\":\""+ kvp2.Value.Subscriber + "\"}");
                        }

                        sb.Append("}");
                    }
                    sb.Append("}");
                } 
                sb.Append(",\"by_subscriber\":");
                {
                    sb.Append("{");
                    var first = true;
                    foreach (var kvp in _allBySubscriber)
                    {
                        if (first) first = false;
                        else sb.Append(",");

                        sb.Append("\"s" + kvp.Key.GetHashCode() + "\":{");

                        var first2 = true;
                        foreach (var kvp2 in kvp.Value)
                        {
                            if (first2) first2 = false;
                            else sb.Append(",");

                            sb.Append("\"s" + kvp2.Key.ToString() + "\":");
                            sb.Append("{\"id\":" + kvp2.Value.ID + "}");
                        }
                        sb.Append("}");
                    }
                    sb.Append("}");
                }
                sb.Append("}");
                return sb.ToString();
            }
        }
    }
}