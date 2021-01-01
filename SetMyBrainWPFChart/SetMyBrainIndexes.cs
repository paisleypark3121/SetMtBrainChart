using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart
{
    public class SetMtBrainIndexes: INotifyPropertyChanged
    {
        #region private fields
        private DateTime _timestamp;
        private float _attention;
        private float _creativity;
        private float _immersion;
        private float _arousal;
        private float _engagement;
        #endregion

        #region get / set
        public DateTime timestamp { get { return _timestamp; } set { _timestamp = value; OnPropertyRaised("timestamp"); } }
        public float attention { get { return _attention; } set { _attention = value; OnPropertyRaised("attention"); } }
        public float creativity { get { return _creativity; } set { _creativity = value; OnPropertyRaised("creativity"); } }
        public float immersion { get { return _immersion; } set { _immersion = value; OnPropertyRaised("immersion"); } }
        public float arousal { get { return _arousal; } set { _arousal = value; OnPropertyRaised("arousal"); } }
        public float engagement { get { return _engagement; } set { _engagement = value; OnPropertyRaised("engagement"); } }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
