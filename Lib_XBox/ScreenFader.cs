using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public class ScreenFader
    {
        public delegate void OnStartFadeIn(ScreenFader screenFader);
        public event OnStartFadeIn StartFadeIn;
        public delegate void OnFinish(ScreenFader screenFader);
        public event OnFinish Finish;

        AlphaBlendHelper abh;
        public Size ScreenSize;
        public enum eState { None, FadingOut, FadingIn }
        public eState State;
        int Speed;

        /// <summary>
        /// Fades the screen out and in again
        /// </summary>
        /// <param name="screenSize"></param>
        /// <param name="speed"></param>
        public ScreenFader(Size screenSize, int speed)
        {
            Speed = speed;
            abh = new AlphaBlendHelper(0, 255, 0, Speed);
            abh.MinMaxReached += new AlphaBlendHelper.OnMinMaxReached(abh_MinMaxReached);
            ScreenSize = screenSize;
            State = eState.FadingOut;
        }

        public void OnlyFadeIn()
        {
            abh.MinMaxReached -= new AlphaBlendHelper.OnMinMaxReached(abh_MinMaxReached);
            abh = new AlphaBlendHelper(0, 255, 255, -Speed);
            State = eState.FadingIn;
            abh.MinMaxReached += new AlphaBlendHelper.OnMinMaxReached(abh_MinMaxReached);
        }

        public void Reset()
        {
            abh.AlphaValue = 0;
            State = eState.FadingOut;
        }

        void abh_MinMaxReached(AlphaBlendHelper abh)
        {
            switch (State)
            {
                case eState.None:
                    throw new Exception("");
                case eState.FadingOut:
                    State = eState.FadingIn;
                    if (StartFadeIn != null)
                        StartFadeIn(this);
                    break;
                case eState.FadingIn:
                    State = eState.None;
                    if (Finish != null)
                        Finish(this);
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Update()
        {
            if(State != eState.None)
                abh.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (State != eState.None)
                spriteBatch.Draw(Common.Black1px, new Rectangle(offset.Xi(), offset.Yi(), ScreenSize.Width, ScreenSize.Height), new Color(255, 255, 255, abh.AlphaValue));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.Zero);
        }
    }
}
