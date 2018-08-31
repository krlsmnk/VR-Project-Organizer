using System;
using System.Collections.Generic;
using System.Text;


namespace EliCDavis.Prosign.Subscription
{
    public class SubscriptionManager
    {
        Dictionary<string, Dictionary<string, List<ISubscriber>>> subscribers;

        Dictionary<string, Dictionary<string, List<ISubscriber>>> oneTimeSubscribers;

        public SubscriptionManager()
        {
            subscribers = new Dictionary<string, Dictionary<string, List<ISubscriber>>>();
            oneTimeSubscribers = new Dictionary<string, Dictionary<string, List<ISubscriber>>>();
        }

        public void SubscribeToRoomUpdates(ISubscriber callback)
        {
            Subscribe("room", "update", callback);
        }

        public void SubscribeOneShot(string service, string method, ISubscriber cb)
        {
            if (oneTimeSubscribers.ContainsKey(service) == false)
            {
                oneTimeSubscribers.Add(service, new Dictionary<string, List<ISubscriber>>());
            }

            if (oneTimeSubscribers[service].ContainsKey(method) == false)
            {
                oneTimeSubscribers[service].Add(method, new List<ISubscriber>());
            }

            oneTimeSubscribers[service][method].Add(cb);
        }

        public void Subscribe(string service, string method, ISubscriber cb)
        {
            if (subscribers.ContainsKey(service) == false)
            {
                subscribers.Add(service, new Dictionary<string, List<ISubscriber>>());
            }

            if (subscribers[service].ContainsKey(method) == false)
            {
                subscribers[service].Add(method, new List<ISubscriber>());
            }

            subscribers[service][method].Add(cb);
        }


        public void DistributeMessage(string service, string method, byte[] body)
        {
            if (oneTimeSubscribers.ContainsKey(service) && oneTimeSubscribers[service].ContainsKey(method))
            {
                var callbacks = oneTimeSubscribers[service][method];
                foreach (var sub in callbacks)
                {
                    sub.Publish(body);
                }
                oneTimeSubscribers[service][method] = new List<ISubscriber>();
            }

            if (subscribers.ContainsKey(service) && subscribers[service].ContainsKey(method))
            {
                var callbacks = subscribers[service][method];
                foreach (var sub in callbacks)
                {
                    sub.Publish(body);
                }
            }
        }

    }

}
