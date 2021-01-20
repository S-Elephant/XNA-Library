using System;
using System.Diagnostics;

namespace XNALib
{
    public static class FPSCounter
    {
        #region Members
        private static Stopwatch stopwatch = Stopwatch.StartNew();
        public static TimeSpan SampleSpan = TimeSpan.FromSeconds(1);
        private static int sampleFrames = 0;

        private static float m_Fps = 0f;
        public static float Fps
        {
            get { return m_Fps; }
            private set { m_Fps = value; }
        }
       
        #endregion

        public static void Update()
        {
            if (stopwatch.Elapsed > SampleSpan)
            {
                // Update FPS value and start next sampling period.
                Fps = (float)sampleFrames / (float)stopwatch.Elapsed.TotalSeconds;

                stopwatch.Reset();
               // stopwatch.Start();
                sampleFrames = 0;
            }
        }

        public static void Draw()
        {
            sampleFrames++;
        }
    }
}
