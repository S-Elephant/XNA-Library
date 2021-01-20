using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Controls
{
    internal class TSTile
    {
        public Rectangle DrawRect;
        public Rectangle SourceRect;

        public TSTile(Point location, int gridSize, Rectangle sourceRect)
        {
            DrawRect = new Rectangle(location.X, location.Y, gridSize, gridSize);
            SourceRect = sourceRect;
        }
    }

    public class TextureSelector : BaseControl, IControl
    {
        #region Members

        /// <summary>
        /// tile size
        /// </summary>
        int GridSize;

        /// <summary>
        /// tilesize + itemspacing
        /// </summary>
        int TotalGridSize;

        /// <summary>
        /// Spacing between the tiles on screen
        /// </summary>
        const int ItemSpacing = 2;

        /// <summary>
        /// The source offset of the image.
        /// </summary>
        public Point SelSource
        {
            get { return Tiles[SelTileIdx.X, SelTileIdx.Y].SourceRect.Location; }
        }

        /// <summary>
        /// Total amount of texture per row in the image
        /// </summary>
        int TexturesPerRow;
        /// <summary>
        /// Total rows in the image
        /// </summary>
        int TotalRows;
        /// <summary>
        /// Total amount of textures in the image.
        /// </summary>
        int TotalTextures;
        /// <summary>
        /// Maximum visible amount of rows in the control
        /// </summary>
        int MaxVisRows;

        /// <summary>
        /// Maximum visible amount of tiles per row in the control
        /// </summary>
        int MaxTilesPerRow;

        /// <summary>
        /// The tiles
        /// </summary>
        TSTile[,] Tiles;

        /// <summary>
        /// The index of the currently selected tile
        /// </summary>
        Point SelTileIdx;

        /// <summary>
        /// Scroll amount of the tiles in the texture selector
        /// </summary>
        Point Scroll;

        /// <summary>
        /// List of textures to cycle through.
        /// </summary>
        List<Texture2DNamed> Textures = new List<Texture2DNamed>();
        int SelTexIdx = 0;
        public Texture2DNamed SelTexture { get { return Textures[SelTexIdx]; } }

        /// <summary>
        /// Browsing through the textures
        /// </summary>
        public Keys NextTextureKey = Keys.Y;
        public Keys PrevTextureKey = Keys.H;
        public Keys NextTileKey = Keys.X;
        public Keys PrevTileKey = Keys.C;

        // Other
        public bool RequireFocusForCycling = false;

        #endregion

        public TextureSelector(Rectangle aabb, int gridSize, params string[] textures)
        {
            AABB = aabb;
            Location = AABB.Location.ToVector2();
            foreach (string texture in textures)
                Textures.Add(new Texture2DNamed(texture));
            MaxVisRows = AABB.Height / (gridSize + ItemSpacing);
            SetTexture(0, gridSize);
        }

        public void SetTexture(int newTexIdx, int gridSize)
        {
            // Scroll through the textures
            if (newTexIdx >= Textures.Count)
                SelTexIdx = 0;
            else if (newTexIdx < 0)
                SelTexIdx = Textures.Count - 1;
            else
                SelTexIdx = newTexIdx;

            // Set variables
            GridSize = gridSize;
            TotalGridSize = GridSize + ItemSpacing;
            SelTileIdx = Point.Zero;
            Scroll = Point.Zero;

            TexturesPerRow = SelTexture.Texture.Width / TotalGridSize;
            TotalRows = SelTexture.Texture.Height / TotalGridSize;
            TotalTextures = TexturesPerRow * TotalRows;
            MaxTilesPerRow = AABB.Width / TotalGridSize;

            // Create new array of tiles
            Tiles = new TSTile[TexturesPerRow, TotalRows];

            for (int y = 0; y < TotalRows; y++)
            {
                for (int x = 0; x < TexturesPerRow; x++)
                {
                    Tiles[x, y] = new TSTile(new Point(x * TotalGridSize, y * TotalGridSize), GridSize, new Rectangle(x * GridSize, y * GridSize, GridSize, GridSize));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                HasFocus = Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB);

                if (HasFocus)
                {
                    if (InputMgr.Instance.Mouse.LeftButtonIsDown)
                    {
                        foreach (TSTile tile in Tiles)
                        {
                            if (Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, tile.DrawRect))
                                SelTileIdx = new Point(tile.SourceRect.X / GridSize, tile.SourceRect.Y / GridSize);
                        }
                    }

                    if (InputMgr.Instance.Keyboard.IsPressed(Keys.Up))
                        Scroll.Y++;
                    else if (InputMgr.Instance.Keyboard.IsPressed(Keys.Right))
                        Scroll.X--;
                    else if (InputMgr.Instance.Keyboard.IsPressed(Keys.Down))
                        Scroll.Y--;
                    else if (InputMgr.Instance.Keyboard.IsPressed(Keys.Left))
                        Scroll.X++;
                }

                if (!RequireFocusForCycling || (HasFocus))
                {
                    if (InputMgr.Instance.Keyboard.IsPressed(NextTextureKey))
                        SetTexture(SelTexIdx + 1, GridSize);
                    else if (InputMgr.Instance.Keyboard.IsPressed(PrevTextureKey))
                        SetTexture(SelTexIdx - 1, GridSize);

                    if (InputMgr.Instance.Keyboard.IsPressed(NextTileKey))
                    {
                        if (SelTileIdx.X < TexturesPerRow - 1)
                        {
                            SelTileIdx.X++;
                        }
                        else if (SelTileIdx.Y < TexturesPerRow - 1)
                        {
                            SelTileIdx = new Point(0, SelTileIdx.Y + 1);
                        }
                        else
                            SelTileIdx = Point.Zero;
                    }
                    else if (InputMgr.Instance.Keyboard.IsPressed(PrevTileKey))
                    {
                        /*  if (SelTileIdx.X > 0)
                              SelTileIdx.X--;// = new Point(SelSource.X - GridSize - ItemSpacing, SelSource.Y);
                          else if (SelTileIdx.Y > 0)
                          {
                              SelTileIdx = new Point((TexturesPerRow - 1) * (GridSize + ItemSpacing), SelTileIdx.Y - 1);
                          }
                          else
                              SelTileIdx = Point.Zero;*/
                    }
                }
            }
        }

        public void Draw()
        {
            if (IsVisible)
            {
                // BG
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px50Trans, AABB, Color.Black);

                // Draw textures
                for (int y = Scroll.Y; y < Math.Min(MaxVisRows, TotalRows); y++)
                {
                    for (int x = Scroll.X; x < Scroll.X + Math.Min(MaxTilesPerRow, TexturesPerRow); x++)
                    {
                        if (x >= 0 && y >= 0 && x < TexturesPerRow && y < TotalRows)// check if x and y are valid at all
                            ControlMgr.Instance.SpriteBatch.Draw(SelTexture.Texture, Tiles[x, y].DrawRect.AddVector2((Scroll.ToVector2() * TotalGridSize) + Location), Tiles[x, y].SourceRect, Color.White);
                    }
                }

                // Draw Selector            
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px50Trans, Tiles[SelTileIdx.X, SelTileIdx.Y].DrawRect.AddVector2((Scroll.ToVector2() * TotalGridSize) + Location), Color.Yellow);

                DrawChildControls();
            }
        }
    }
}
