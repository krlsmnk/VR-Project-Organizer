using System;

namespace EliCDavis.Prosign.Subscription
{
    public static class SubscriberFactory
    {

        public static ISubscriber MakeSubscriber(Action subscriber)
        {
            return new VoidSubscriber(subscriber);
        }

        public static ISubscriber MakeSubscriber(Action<string> subscriber)
        {
            return new StringSubscriber(subscriber);
        }

        public static ISubscriber MakeSubscriber(Action<byte[]> subscriber)
        {
            return new ByteArraySubscriber(subscriber);
        }

    }

}
