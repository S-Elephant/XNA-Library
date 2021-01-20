using System;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public class FlashWhite
    {
        #region Members
        //The minimum value for each of the red, green, and blue color components 
        public const int colorMinimumValue = 25;

        //The maximum value for each of the red, green, and blue color components 
        public const int colorMaximumValue = 230;

        //The maximum value for each of the red, green, and blue color components above their original values  
        public const int colorPositiveRange = 25;

        //The minimum value for each of the red, green, and blue color components below their original values
        public const int colorNegativeRange = 25;

        /// <summary>
        /// The original color of the object.
        /// </summary>
        public Color InitialColor;

        /// <summary>
        /// The higher the value the faster it flashes.
        /// </summary>
        public double FlashSpeed = 16;

        /// <summary>
        /// The effective flash color. Use this one to draw the sprite or whatever you use it for.
        /// </summary>
        public Color DrawColor = Color.Black;

        /// <summary>
        /// The time in ms to flash. After it expires it automatically stops flashing.
        /// </summary>
        private int FlashTimeInMS = -1;

        /// <summary>
        /// Indicates if it is currently flashing.
        /// </summary>
        public bool IsFlashing = false;

        /// <summary>
        /// The timer to keep track of how long it is flashing.
        /// </summary>
        private TimeSpan Timer = new TimeSpan();
        #endregion

        public FlashWhite()
        {
        }

        /// <summary>
        /// Either starts flashing or if it was already flashing it resets the flash-timer.
        /// </summary>
        /// <param name="initialColor"></param>
        /// <param name="flashTimeInMS"></param>
        public void StartFlash(Color initialColor, int flashTimeInMS)
        {
            if (!IsFlashing)
            {
                DrawColor = InitialColor = initialColor;
                IsFlashing = true;
                FlashTimeInMS = flashTimeInMS;
                Timer = TimeSpan.Zero;
            }
            else
                ResetTimer();
        }

        public void ResetTimer()
        {
            Timer = TimeSpan.Zero;
        }

        public void StopFlash()
        {
            DrawColor = InitialColor;
            IsFlashing = false;
        }

        public void Update(GameTime gameTime)
        {
            float pulseCycle = (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * FlashSpeed) * 0.5f) + 0.5f);

            DrawColor = new Color(pulseCycle, pulseCycle, pulseCycle);

            Timer += gameTime.ElapsedGameTime;
            if (Timer.TotalMilliseconds >= FlashTimeInMS)
                StopFlash();
        }
    }
}
