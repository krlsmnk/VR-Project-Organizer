using System;

namespace EliCDavis.Prosign.Subscription
{
    public interface ISubscriber
    {

        void Publish(byte[] message);

    }

}
