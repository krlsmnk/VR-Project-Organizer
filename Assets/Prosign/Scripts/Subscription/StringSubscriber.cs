using System;

namespace EliCDavis.Prosign.Subscription
{
    public class StringSubscriber: ISubscriber
    {

        Action<string> subscriber;

        public StringSubscriber(Action<string> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void Publish(byte[] message){
            subscriber(System.Text.Encoding.UTF8.GetString(message));
        }

    }

}
