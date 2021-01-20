using Microsoft.Xna.Framework;

namespace XNALib
{
    /// <summary>
    /// Warning: Does not work (in XNA 4.0) with the Color.White and other high color values likes Color(250,250,250,a). Use the FaderWhite class instead for these cases
    /// </summary>
    public class Fader
    {
        public delegate void OnFadeCompleteEventHandler();
        public event OnFadeCompleteEventHandler OnFadeComplete;
   
        #region Members
        private int m_AlphaValue = 0;
        public int AlphaValue
        {
            get { return m_AlphaValue; }
            set { m_AlphaValue = value; }
        }

        public bool AutoResets = false;

        private const double BaseFadeDelay = 0.35;
        private double m_FadeDelay = BaseFadeDelay;
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

        /// <summary>
        /// Usage: spriteBatch.Draw(Texture, new Rectangle(0, 0, Texture.Width, Texture.Height), new Color(0, 0, 0, (byte)MathHelper.Clamp(Fader.AlphaValue, 0, 255)));
        /// </summary>
        /// <param name="fadeSpeed"></param>
        public Fader(int fadeSpeed)
        {
            FadeSpeed = fadeSpeed;
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
