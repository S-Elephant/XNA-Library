using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class SimpleASprite
    {
        #region Members
        public Texture2D Texture;
        public Point FrameSize;
        public Point CurrentFrame = new Point(0, 0);
        public Point SheetSize;// = new Point(4, 4); // the sheet has 4 frames, by 4 frames
        private TimeSpan Delay = new TimeSpan();
        public int AnimationDelayInMS;
        public bool IsDisposed = false;
        public bool LoopOnce = false;
        public Vector2 Location;
        public bool IsAnimating = true;
        public Vector2 ExtraDrawOffset = Vector2.Zero;
        public SpriteEffects Effects = SpriteEffects.None;
        public Color FlashColor = Color.White;
        public float DirectionTolerance = 0.20f;
        public int FrameOffsetY = 0;
        public Vector2 CenterLocation
        {
            get { return new Vector2(Location.X + FrameSize.X / 2, Location.Y + FrameSize.Y / 2); }
            set { Location = new Vector2(value.X + FrameSize.X / 2, value.Y + FrameSize.Y / 2); }
        }
        /// <summary>
        /// Returns a rectangle created from the current Location and the FrameSize.
        /// </summary>
        public Rectangle FrameColRect { get { return new Rectangle(Location.Xi(), Location.Yi(), FrameSize.X, FrameSize.Y); } }
        public FlashWhite FlashWhite = new FlashWhite();
        
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="texture"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="frameCntX"></param>
        /// <param name="frameCntY"></param>
        /// <param name="animationDelayInMS">Delay in ms between 2 frames. Use int.MaxValue to set the IsAnimation to false.</param>
        public SimpleASprite(Vector2 location, Texture2D texture, int frameWidth, int frameHeight, int frameCntX, int frameCntY, int animationDelayInMS)
        {
            Location = location;
            Texture = texture;
            AnimationDelayInMS = animationDelayInMS;
            if (AnimationDelayInMS == int.MaxValue)
                IsAnimating = false;
            FrameSize = new Point(frameWidth, frameHeight);
            SheetSize = new Point(frameCntX, frameCntY);
        }

        public SimpleASprite(Vector2 location, string texture, int frameWidth, int frameHeight, int frameCntX, int frameCntY, int animationDelayInMS)
        {
            Location = location;
            Texture = Common.str2Tex(texture);
            AnimationDelayInMS = animationDelayInMS;
            if (AnimationDelayInMS == int.MaxValue)
                IsAnimating = false;
            FrameSize = new Point(frameWidth, frameHeight);
            SheetSize = new Point(frameCntX, frameCntY);
        }
        #endregion

        public void StartFlash(int timeInMS)
        {
            FlashWhite.StartFlash(FlashColor, timeInMS);
        }

        public void RandomizeStartFrame()
        {
            CurrentFrame.X = Maths.RandomNr(0, SheetSize.X - 1);
        }

        /// <summary>
        /// Resets the frame to 0,0 and sets IsDisposed to false.
        /// </summary>
        public void Reset()
        {
            IsDisposed = false;
            CurrentFrame = Point.Zero;
        }

        #region SetDirection
        /// <summary>
        /// Works only for a sprite that has 8 directions and each direction on it's own row.
        /// </summary>
        /// <param name="faceThisPoint"></param>
        public void SetDirection(Vector2 location, Vector2 faceThisPoint)
        {
            CurrentFrame.Y = Misc.GetAnimationDirection(Maths.GetMoveDir(location, faceThisPoint), DirectionTolerance);
        }

        /// <summary>
        /// Works only for a sprite that has 8 directions and each direction on it's own row.
        /// </summary>
        /// <param name="faceThisPoint"></param>
        public void SetDirectionByDir(Vector2 moveDir)
        {
            CurrentFrame.Y = Misc.GetAnimationDirection(moveDir, DirectionTolerance);
        }

        /// <summary>
        /// Works only for a sprite that has 8 directions and each direction on it's own row.
        /// </summary>
        /// <param name="faceThisPoint"></param>
        public void SetDirection(Vector2 faceThisPoint)
        {
            CurrentFrame.Y = Misc.GetAnimationDirection(Maths.GetMoveDir(Location, faceThisPoint), DirectionTolerance);
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            if (IsAnimating)
            {
                Delay += gameTime.ElapsedGameTime;
                if (Delay.TotalMilliseconds >= AnimationDelayInMS)
                {
                    Delay = new TimeSpan();

                    ++CurrentFrame.X; // move the source rectangle 1 frame to the right
                    if (CurrentFrame.X >= SheetSize.X)
                    {
                        CurrentFrame.X = 0; //once the source rectangle moves 4 spaces to the right, reset to the first column
                        if (SheetSize.Y > 1)
                        {
                            ++CurrentFrame.Y; //move the source rectangle down one
                            if (CurrentFrame.Y >= SheetSize.Y)
                            {
                                if (LoopOnce)
                                    IsDisposed = true;
                                else
                                    CurrentFrame.Y = 0; //when at the bottom of the sheet reset the source to the top
                            }
                        }
                        else if (LoopOnce)
                            IsDisposed = true;
                    }
                }
            }

            if (FlashWhite.IsFlashing)
            {
                FlashWhite.Update(gameTime);
                FlashColor = FlashWhite.DrawColor;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Draw(spriteBatch, Vector2.Zero, 1, color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, float depth, Color color)
        {
            spriteBatch.Draw(Texture, Location + offset + ExtraDrawOffset, new Rectangle(CurrentFrame.X * FrameSize.X, //first point starts at 0, 0
        (CurrentFrame.Y + FrameOffsetY) * FrameSize.Y, //second at 0, 0
        FrameSize.X, //third, 128, 0
        FrameSize.Y), // fourth, 0,128
        color, 0, Vector2.Zero,
        1, Effects, depth);

        }
    }
}