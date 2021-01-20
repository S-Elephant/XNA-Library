using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public class HexTileMoveCostComparer : IComparer<HexTile>
    {
        public int Compare(HexTile x, HexTile y)
        {
            return x.AStar.F - y.AStar.F;
        }
    }

    /// <summary>
    /// F: Total movement cost (G+H).
    /// G: Movement cost from start to this node.
    /// H: estimated movement cost from this node to the end-node.
    /// </summary>
    public struct AStarContainer
    {
        public int G, H;
        public int F
        {
            get { return G + H; }
        }
    }

    public class HexTile
    {
        public Texture2D Texture;
        public Point GridIdx;
        Vector2 DrawLoc;
        public Color DrawColor = Color.White;
        HexGrid Grid;
        public Vector2 ScreenCenterLoc { get { return DrawLoc + Grid.TopLeft + new Vector2(Grid.TileWidth / 2, Grid.TileHeight / 2); } }

        // Neighbours
        public HexTile NB_NW;
        public HexTile NB_NE;
        public HexTile NB_E;
        public HexTile NB_W;
        public HexTile NB_SW;
        public HexTile NB_SE;

        // Pathfinding
        //public int BaseCost = 100; // The cost for moving onto this tile.
        public AStarContainer AStar = new AStarContainer();
        //public bool IsImpassable { get { return BaseCost == int.MaxValue; } }
        public bool IsImpassable = false;

        public HexTile(Texture2D texture, HexGrid grid, Point gridIdx)
        {
            Texture = texture;
            Grid = grid;
            GridIdx = gridIdx;

            int offsetX = 0;
            if (!gridIdx.Y.IsEven())
                offsetX = Grid.TileWidth / 2;

            DrawLoc = new Vector2(gridIdx.X * Grid.TileWidth + offsetX, gridIdx.Y * (Grid.TileHeight * 0.75f));
        }

        public List<HexTile> GetNeighbours()
        {
            return new List<HexTile>() { NB_E, NB_NE, NB_NW, NB_SE, NB_SW, NB_W };
        }

        public List<HexTile> GetPassableneigboars()
        {
            List<HexTile> neighboars = GetNeighbours(); // Get the neigboars
            for (int i = 0; i <= neighboars.Count - 1; i++)
            {
                if (neighboars[i] == null)
                {
                    neighboars.RemoveAt(i); // Filter the null's out.              
                    i--;
                }
                else
                {
                    if (neighboars[i].IsImpassable)
                    {
                        neighboars.RemoveAt(i); // filter the impassable ones out (the ones with G == int.MaxValue).
                        i--;
                    }
                }
            }
            return neighboars;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawLoc + Grid.TopLeft, DrawColor);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(Texture, DrawLoc + Grid.TopLeft + offset, DrawColor);
        }

        public void DrawCellCoordinates(SpriteBatch spriteBatch, Vector2 cameraOffset, SpriteFont font, Color textColor)
        {
            DrawInfo(string.Format("{0},{1}", GridIdx.X, GridIdx.Y), spriteBatch, cameraOffset, font, textColor);
        }

        public void DrawFGH(SpriteBatch spriteBatch, Vector2 cameraOffset, SpriteFont font, Color textColor)
        {
            //DrawInfo(string.Format("{0},{1},{2}", AStar.F,AStar.G,AStar.H),spriteBatch,cameraOffset,font,textColor);
            DrawInfo(string.Format("{0} {1}",AStar.F,  AStar.H), spriteBatch, cameraOffset, font, textColor);
        }

        private void DrawInfo(string text, SpriteBatch spriteBatch, Vector2 cameraOffset, SpriteFont font, Color textColor)
        {
            Vector2 fontMeasure = font.MeasureString(text);
            spriteBatch.DrawString(font, text, ScreenCenterLoc - new Vector2(fontMeasure.X / 2, fontMeasure.Y / 2) + cameraOffset, textColor);
        }
    }
}
