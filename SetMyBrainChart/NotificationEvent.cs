using System;

namespace SetMyBrainChart
{
    public class NotificationEvent
    {

        public NeuroskyData NotificationMessage { get; private set; }

        public DateTime NotificationDate { get; private set; }

        public NotificationEvent(DateTime _dateTime, NeuroskyData _message)
        {
            NotificationDate = _dateTime;
            NotificationMessage = _message;
        }

    }
}