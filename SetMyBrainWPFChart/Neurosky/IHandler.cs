using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Neurosky
{
    public interface IHandler
    {
        void HandlePoorSignal(object parameters);
        void HandleFrequencies(object parameters);
        void HandleIndexes(object parameters);
        void HandleSlopes(object parameters);

        void HandleFlow(object parameters);
    }
}
