using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace XNALib
{
    /* Example usage
            foreach (Viewport vp in ViewPortMgr.ViewPorts)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Resolution.GetScaleMatrix() * VPTranslation); // example translation: Engine.VPTranslations[(int)LogicalPlayerIdx] = Matrix.CreateTranslation(Camera.X, Camera.Y, 0);
                GraphicsDevice.Viewport = vp;
                Engine.ActiveState.Draw();
                spriteBatch.End();
            }
    */

    /// <summary>
    /// WARNING: Not fully tested and the 3-player implementation is not finished and will throw an exception.
    /// </summary>
    public static class ViewPortMgr
    {
        public static List<Viewport> ViewPorts = new List<Viewport>();
        public enum eViewType { Normal, Vertical }
        static eViewType ViewType;

        public static void SetViewPorts(int bufferWidth, int bufferHeight, int nrOfViewPorts, eViewType viewType)
        {
            ViewType = viewType;
            if (viewType == eViewType.Normal)
            {
                #region normal
                ViewPorts.Clear();
                switch (nrOfViewPorts)
                {
                    case 1:
                        ViewPorts.Add(new Viewport(0, 0, bufferWidth, bufferHeight));
                        break;
                    case 2:
                        ViewPorts.Add(new Viewport(0, 0, bufferWidth, bufferHeight / 2));
                        ViewPorts.Add(new Viewport(0, bufferHeight / 2, bufferWidth, bufferHeight / 2));
                        break;
                    case 3:
                        throw new NotImplementedException();
                    case 4:
                        Viewport vp = new Viewport();

                        vp.X = 0;
                        vp.Y = 0;
                        vp.Width = bufferWidth / 2;
                        vp.Height = bufferHeight / 2;

                        ViewPorts.Add(vp);

                        vp.X = bufferWidth / 2; ;
                        vp.Y = 0;
                        vp.Width = bufferWidth / 2;
                        vp.Height = bufferHeight / 2;

                        ViewPorts.Add(vp);

                        vp.X = 0;
                        vp.Y = bufferHeight / 2;
                        vp.Width = bufferWidth / 2;
                        vp.Height = bufferHeight / 2;

                        ViewPorts.Add(vp);

                        vp.X = bufferWidth / 2;
                        vp.Y = bufferHeight / 2;
                        vp.Width = bufferWidth / 2;
                        vp.Height = bufferHeight / 2;

                        ViewPorts.Add(vp);
                        break;
                    default:
                        throw new CaseStatementMissingException();
                }
                #endregion
            }
            else
            {
                #region vertical
                ViewPorts.Clear();
                switch (nrOfViewPorts)
                {
                    case 1:
                        ViewPorts.Add(new Viewport(0, 0, bufferWidth, bufferHeight));
                        break;
                    case 2:
                        ViewPorts.Add(new Viewport(0, 0, bufferWidth / 2, bufferHeight));
                        ViewPorts.Add(new Viewport(bufferWidth / 2, 0, bufferWidth / 2, bufferHeight));
                        break;
                    case 3:
                        throw new NotImplementedException();
                    case 4:
                        ViewPorts.Add(new Viewport(0, 0, bufferWidth / 4, bufferHeight));
                        ViewPorts.Add(new Viewport(bufferWidth / 4, 0, bufferWidth / 4, bufferHeight));
                        ViewPorts.Add(new Viewport(bufferWidth / 2, 0, bufferWidth / 4, bufferHeight));
                        ViewPorts.Add(new Viewport(bufferWidth / 2 + bufferWidth / 4, 0, bufferWidth / 4, bufferHeight));
                        break;
                    default:
                        throw new CaseStatementMissingException();
                }
                #endregion
            }
        }

        public static void DrawBorder(SpriteBatch spriteBatch, int bufferWidth, int bufferHeight)
        {
            switch (ViewPorts.Count)
            {
                case 1:
                    // Do nothing
                    break;
                case 2:
                    if (ViewType == eViewType.Normal)
                        spriteBatch.Draw(Common.White1px, new Rectangle(0, bufferHeight / 2 - 1, bufferWidth, 3), Color.Black);
                    else
                        spriteBatch.Draw(Common.White1px, new Rectangle(bufferWidth / 2 - 1, 0, 3, bufferHeight), Color.Black);
                    break;
                case 3:
                    throw new NotImplementedException();
                case 4:
                    if (ViewType == eViewType.Normal)
                    {
                        // vertical
                        spriteBatch.Draw(Common.White1px, new Rectangle(bufferWidth / 2 - 1, 0, 3, bufferHeight), Color.Black);
                        // horizontal
                        spriteBatch.Draw(Common.White1px, new Rectangle(0, bufferHeight / 2 - 1, bufferWidth, 3), Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(Common.White1px, new Rectangle(bufferWidth / 3 - 1, 0, 3, bufferHeight), Color.Black);
                        spriteBatch.Draw(Common.White1px, new Rectangle((bufferWidth / 3) * 2 - 1, 0, 3, bufferHeight), Color.Black);
                    }
                    break;
                default:
                    throw new Exception("Supports only 1-4 players");
            }
        }        
    }
}
