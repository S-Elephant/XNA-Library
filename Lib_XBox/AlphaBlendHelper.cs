using System;

namespace XNALib
{
    public class AlphaBlendHelper
    {
        public delegate void OnMinMaxReached(AlphaBlendHelper abh);
        public event OnMinMaxReached MinMaxReached;

        int Min, Max, Incrementer;
        public int AlphaValue;
        public bool Loop = true;        
        private int m_LoopCnt = new int();
        /// <summary>
        /// The number of times to flash (a flash is one countdown or one countup. not both)
        /// </summary>
        public int LoopCnt
        {
            get { return m_LoopCnt; }
            set { m_LoopCnt = StartLoopCnt = value; }
        }
        private int StartLoopCnt = 0;
       
        public bool IsDone = false;
        public int StartAlpha;

        /// <summary>
        /// REQUIRES: SpriteBatch: BlendState.NonPremultiplied or it might not work.
        /// </summary>
        /// <param name="minAlpha"></param>
        /// <param name="maxAlpha"></param>
        /// <param name="speed">usually between -3 to 3</param>
        public AlphaBlendHelper(int minAlpha, int maxAlpha, int startAlpha, int speed)
        {
            if (minAlpha < 0 || maxAlpha > 255 || startAlpha < 0 || startAlpha > 255)
                throw new ArgumentOutOfRangeException();

            Min = minAlpha;
            Max = maxAlpha;
            AlphaValue = StartAlpha = startAlpha;
            Incrementer = speed;
        }

        public void Reset()
        {
            IsDone = false;
            AlphaValue = StartAlpha;
            m_LoopCnt = StartLoopCnt;
        }

        public void Update()
        {
            if (AlphaValue + Incrementer > Max || AlphaValue + Incrementer < Min)
            {
                if (Loop || LoopCnt > 0)
                {
                    Incrementer *= -1;
                    if(LoopCnt > 0)
                        m_LoopCnt--;
                }
                else
                    IsDone = true;

                if (MinMaxReached != null)
                    MinMaxReached(this);
            }
            AlphaValue += Incrementer;
        }
    }
}
