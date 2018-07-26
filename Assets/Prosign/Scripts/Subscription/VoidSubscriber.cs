using System;

namespace EliCDavis.Prosign.Subscription
{
    public class VoidSubscriber: ISubscriber
    {

        Action subscriber;

        public VoidSubscriber(Action subscriber)
        {
            this.subscriber = subscriber;
        }

        public void Publish(byte[] message){
            subscriber();
        }

    }

}
