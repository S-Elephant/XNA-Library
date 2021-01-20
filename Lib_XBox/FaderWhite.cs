using Microsoft.Xna.Framework;

namespace XNALib
{
    public class FaderWhite
    {
        public delegate void OnFadeCompleteEventHandler();
        public event OnFadeCompleteEventHandler OnFadeComplete;
   
        #region Members
        private Color m_FadeColor = new Color(0,0,0,0);
        public Color FadeColor
        {
            get { return m_FadeColor; }
            private set { m_FadeColor = value; }
        }

        private Color m_FadeColorInversed = Color.White;
        public Color FadeColorInversed
        {
            get { return m_FadeColorInversed; }
            private set { m_FadeColorInversed = value; }
        }

        private int m_AlphaValue = 0;
        private int AlphaValue
        {
            get { return m_AlphaValue; }
            set { m_AlphaValue = value; }
        }

        public bool AutoResets = false;

        public double BaseFadeDelay;
        private double m_FadeDelay;
        private double FadeDelay
        {
            get { return m_FadeDelay; }
            set { m_FadeDelay = value; }
        }

        private bool m_IsFading = true;
        public bool IsFading
        {
            get { return m_IsFading; }
            set { m_IsFading = value; }
        }

        public int FadeSpeed { get; set; }
        #endregion

        public FaderWhite(int fadeSpeed, double baseFadeDelay)
        {
            FadeSpeed = fadeSpeed;
            BaseFadeDelay = FadeDelay = baseFadeDelay;
        }

        public void Reset()
        {
            AlphaValue = 0;
            IsFading = false;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFading && AlphaValue < 255)
            {
                FadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (FadeDelay <= 0)
                {
                    FadeDelay = BaseFadeDelay;
                    //Increment/Decrement the fade value for the image
                    AlphaValue += FadeSpeed;
                    FadeColor = new Color(AlphaValue, AlphaValue, AlphaValue, AlphaValue);
                    FadeColorInversed = new Color(255 - AlphaValue, 255 - AlphaValue, 255 - AlphaValue, 255 - AlphaValue);

                    if (AlphaValue >= 255) // if true then the fade is at max
                    {
                        if (OnFadeComplete != null)
                            OnFadeComplete();
                        if (AutoResets)
                            Reset();
                    }
                }
            }
        }
    }
}
