using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNALib
{
    /// <summary>
    /// Automatically resets itself.
    /// </summary>
    public class CycleDelay
    {
        public int DelayInCycles;
        public int DelayCnt = 0;
        public bool IsReady { get { return DelayCnt == DelayInCycles; } }
        
        public CycleDelay(int delayInCycles)
        {
            DelayInCycles = delayInCycles;
        }

        public void Update()
        {
            if (DelayCnt != DelayInCycles)
                DelayCnt++;
            else
                DelayCnt = 0;
        }
    }
}
