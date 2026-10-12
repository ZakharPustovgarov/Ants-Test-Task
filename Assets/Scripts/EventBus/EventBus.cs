using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimpleEventBus
{
    public interface IEvent { }

    public interface IEventBus
    {
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }

    public class EventBus : IEventBus
    {
        private static EventBus instance = null;
        public static EventBus Instance { 
            get {
                if (instance == null) instance = new EventBus(); 
                return instance;
            } 
        }

        private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);

            _handlers.AddOrUpdate(eventType,
                _ => new List<Delegate> { handler },
                (_, list) =>
                {
                    lock (list)
                    {
                        if (!list.Contains(handler))
                        {
                            list.Add(handler);
                        }
                    }
                    return list;
                });
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEvent);

            if (_handlers.TryGetValue(eventType, out var list))
            {
                lock (list)
                {
                    list.Remove(handler);
                }
            }
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            var eventType = typeof(TEvent);

            if (_handlers.TryGetValue(eventType, out var list))
            {
                List<Delegate> copiedHandlers;

                lock (list)
                {
                    copiedHandlers = new List<Delegate>(list);
                }

                foreach (var handler in copiedHandlers)
                {
                    ((Action<TEvent>)handler)(@event);
                }
            }
        }
    }
}
