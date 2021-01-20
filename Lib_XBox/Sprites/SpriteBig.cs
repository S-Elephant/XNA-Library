using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib
{
    public class SBTexture
    {
        public Texture2D Texture;
        public int AnimationDelayInMS;

        public SBTexture(Texture2D texture, int animationDelayInMS)
        {
            Texture = texture;
            AnimationDelayInMS = animationDelayInMS;
        }
    }

    /// <summary>
    /// Author: C.a.r.l.o..v.o.n..R.a.n.z.o.w
    /// A sprite class to handle large sprites (max 2048x2048 per frame on non-HiDef). Every frame has its own texture.
    /// </summary>
    public class SpriteBig : IXNADispose
    {
        // Loop once
        public delegate void OnLoopIsDone(SpriteBig sprite);
        public event OnLoopIsDone LoopIsDone;
        public bool LoopOnce = false;

        // Spriting
        private TimeSpan Delay = new TimeSpan();
        public int CurrentFrame = 0;
        Dictionary<string, List<SBTexture>> Sequences = new Dictionary<string, List<SBTexture>>();
        private string CurrentAnimation = null;
        public bool AnimationIsPaused = false;

        // Drawing
        public Vector2 Location;
        public Vector2 ExtraDrawOffset = Vector2.Zero;
        public Color DrawColor = Color.White;

        // Misc
        public bool IsDisposed { get; set; }
        public int FrameWidth { get { return Sequences[CurrentAnimation][CurrentFrame].Texture.Width; } }
        public int FrameHeight { get { return Sequences[CurrentAnimation][CurrentFrame].Texture.Height; } }
        public SpriteEffects Effects = SpriteEffects.None;

        public SpriteBig(Vector2 location)
        {
            Location = location;
        }

        public void AddAnimation(string animationName, int animationDelayInMS, params string[] textures)
        {
            List<SBTexture> list = new List<SBTexture>();
            foreach (string texture in textures)
                list.Add(new SBTexture(Common.str2Tex(texture), animationDelayInMS));
            Sequences.Add(animationName, list);
            
            if (CurrentAnimation == null)
                CurrentAnimation = animationName;
        }

        public void SwitchAnimation(string newAnimation)
        {
            SwitchAnimation(newAnimation, true);
        }

        public void SwitchAnimation(string newAnimation, bool resetFrame)
        {
            CurrentAnimation = newAnimation;
            if (resetFrame)
                CurrentFrame = 0;
            else if (CurrentFrame >= Sequences[CurrentAnimation].Count)
                CurrentFrame = Sequences[CurrentAnimation].Count - 1;
            
        }

        public void Update(GameTime gameTime)
        {
            if (!AnimationIsPaused && !IsDisposed)
            {
                Delay += gameTime.ElapsedGameTime;
                if (Delay.TotalMilliseconds >= Sequences[CurrentAnimation][CurrentFrame].AnimationDelayInMS)
                {
                    Delay = new TimeSpan();

                    CurrentFrame++;
                    if (CurrentFrame >= Sequences[CurrentAnimation].Count)
                    {
                        if (LoopIsDone != null)
                            LoopIsDone(this);
                        if (LoopOnce)
                        {
                            IsDisposed = true;
                            CurrentFrame--; // Set the current frame to the last frame of this animation again.
                        }
                        else
                            CurrentFrame = 0;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraOffset)
        {
            spriteBatch.Draw(Sequences[CurrentAnimation][CurrentFrame].Texture, Location + ExtraDrawOffset + cameraOffset, null, DrawColor, 0f, Vector2.Zero, 1f, Effects, 1f);
        }
    }
}
