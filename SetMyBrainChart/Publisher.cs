using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SetMyBrainChart
{
    public class Publisher
    {
        private static Random random = new Random();

        public string PublisherName { get; private set; }
        public int NotificationInterval { get; private set; }

        public delegate void Notify(Publisher p, NotificationEvent e);

        // declare an event variable of the delegate function
        public event Notify OnPublish;

        // class constructor
        public Publisher(string _publisherName, int _notificationInterval)
        {
            PublisherName = _publisherName;
            NotificationInterval = _notificationInterval;
        }

        //publish function publishes a Notification Event
        public void Publish()
        {
            while (true)
            {
                // fire event after certain interval
                Thread.Sleep(NotificationInterval);

                if (OnPublish != null)
                {   
                    NeuroskyData message = new NeuroskyData()
                    {
                        timestamp = DateTime.Now,
                        alpha1 = random.Next(1,20),
                        alpha2 = random.Next(1, 20)
                    };

                    NotificationEvent notificationObj = new NotificationEvent(DateTime.Now, message);
                    OnPublish(this, notificationObj);
                }
                Thread.Yield();
            }
        }
    }
}
