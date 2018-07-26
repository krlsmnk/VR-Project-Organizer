using System;

namespace EliCDavis.Prosign.Subscription
{
    public class ByteArraySubscriber: ISubscriber
    {

        Action<byte[]> subscriber;

        public ByteArraySubscriber(Action<byte[]> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void Publish(byte[] message){
            subscriber(message);
        }

    }

}
