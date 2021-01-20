using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public class HexGrid
    {
        #region Members
        // Boundaries and such
        public int Width, Height, TileWidth, TileHeight;
        private FRect m_GridArea;
        
        // Tile containers
        public List<HexTile> Tiles;
        public HexTile[,] TileArray;
        int ArrayWidth, ArrayHeight;

        // Selecting single tile
        private HexTile m_SelectedTile = null;
        public HexTile SelectedTile
        {
            get { return m_SelectedTile; }
            set
            {
                PreviousSelectedTile = m_SelectedTile;
                m_SelectedTile = value;
            }
        }
        public HexTile PreviousSelectedTile = null;

        // Selecting multiple tiles
        private List<HexTile> m_SelectedTiles = new List<HexTile>();
        public List<HexTile> SelectedTiles
        {
            get { return m_SelectedTiles; }
            set
            {
                PreviousSelectedTiles = m_SelectedTiles;
                m_SelectedTiles = value;
            }
        }
        public List<HexTile> PreviousSelectedTiles = new List<HexTile>();

        // Misc
        Texture2D DefaultTileTex;
        public Vector2 TopLeft;
        public Texture2D BGTex = null;
        public Vector2 DrawOffset = Vector2.Zero;

        #endregion

        #region Constructor and init
        public HexGrid(Vector2 topLeft, int width, int height, int tileWidth, int tileHeight, string defaultTileTexture)
        {
            TopLeft = topLeft;
            Width = width;
            Height = height;
            ArrayWidth = width / tileWidth - 1; // -1 because otherwise the last tile in the odd rows would overlap with 50%.
            ArrayHeight = (int)(height / (tileHeight * 0.75f));
            TileArray = new HexTile[ArrayWidth, ArrayHeight];

            m_GridArea = new FRect(topLeft, width, height);

            DefaultTileTex = Common.str2Tex(defaultTileTexture);
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Create();
            SetNeigbours();
        }

        private void Create()
        {
            Tiles = new List<HexTile>();
            for (int y = 0; y < ArrayHeight; y++)
            {
                for (int x = 0; x < ArrayWidth; x++)
                {
                    HexTile newTile = new HexTile(DefaultTileTex, this, new Point(x, y));
                    Tiles.Add(newTile);
                    TileArray[x, y] = newTile;
                }
            }
        }

        public void SetNeigbours()
        {
            foreach (HexTile t in Tiles)
            {
                #region East and West is the same for the even and odd rows.
                // E
                if (t.GridIdx.X < ArrayWidth - 1)
                    t.NB_E = TileArray[t.GridIdx.X + 1, t.GridIdx.Y];
                // W
                if (t.GridIdx.X > 0)
                    t.NB_W = TileArray[t.GridIdx.X - 1, t.GridIdx.Y];
                #endregion

                if (t.GridIdx.Y.IsEven())
                {
                    // NW
                    if (t.GridIdx.X > 0 && t.GridIdx.Y > 0)
                        t.NB_NW = TileArray[t.GridIdx.X - 1, t.GridIdx.Y - 1];
                    // NE
                    if (t.GridIdx.X < ArrayWidth && t.GridIdx.Y > 0)
                        t.NB_NE = TileArray[t.GridIdx.X, t.GridIdx.Y - 1];
                    // SW
                    if (t.GridIdx.X > 0 && t.GridIdx.Y < ArrayHeight - 1)
                        t.NB_SW = TileArray[t.GridIdx.X - 1, t.GridIdx.Y + 1];
                    // SE
                    if (t.GridIdx.X < ArrayWidth && t.GridIdx.Y < ArrayHeight - 1)
                        t.NB_SE = TileArray[t.GridIdx.X, t.GridIdx.Y + 1];
                }
                else
                {
                    // NW
                    if (t.GridIdx.X >= 0 && t.GridIdx.Y > 0)
                        t.NB_NW = TileArray[t.GridIdx.X, t.GridIdx.Y - 1];
                    // NE
                    if (t.GridIdx.X < ArrayWidth - 1 && t.GridIdx.Y > 0)
                        t.NB_NE = TileArray[t.GridIdx.X + 1, t.GridIdx.Y - 1];
                    // SW
                    if (t.GridIdx.X >= 0 && t.GridIdx.Y < ArrayHeight - 1)
                        t.NB_SW = TileArray[t.GridIdx.X, t.GridIdx.Y + 1];
                    // SE
                    if (t.GridIdx.X < ArrayWidth - 1 && t.GridIdx.Y < ArrayHeight - 1)
                        t.NB_SE = TileArray[t.GridIdx.X + 1, t.GridIdx.Y + 1];
                }
            }
        }

        #endregion

        #region FindMoveArea
        /// <summary>
        /// Finds all tiles it can reach with the given movespeed and avoiding unpassable tiles.
        /// Note: This algorithm costs a lot of cpu. Any movespeed between 1-7 is fine but higher might cost too much.
        /// http://forums.create.msdn.com/forums/t/87087.aspx might have a better solution for the problem.
        /// </summary>
        /// <returns>The tiles that can be reached.</returns>
        public List<HexTile> FindMoveArea(HexTile start, int moveSpeed)
        {
            List<HexTile> neighboars = start.GetPassableneigboars(); // Contains the neighboars of the start tile
            List<HexTile> result = new List<HexTile>();
            LinkedList<HexTile> pathWalked = new LinkedList<HexTile>();
            pathWalked.AddFirst(start);
            foreach (HexTile startNeighboar in neighboars)
            {
                int moveSpeedCost = (int)Math.Ceiling(startNeighboar.AStar.G / (float)100);
                if (moveSpeed >= moveSpeedCost)
                    FindMoveArea(ref result, ref pathWalked, startNeighboar, moveSpeed - moveSpeedCost);
            }
            return result;
        }

        /// <summary>
        /// The recursive procedure
        /// </summary>
        /// <param name="result"></param>
        /// <param name="pathwalked"></param>
        /// <param name="currentTile"></param>
        /// <param name="movespeedLeft"></param>
        private void FindMoveArea(ref List<HexTile> result, ref LinkedList<HexTile> pathwalked, HexTile currentTile, int movespeedLeft)
        {
            List<HexTile> neighboars = currentTile.GetPassableneigboars();
            result.Add(currentTile);
            pathwalked.AddLast(currentTile);
            foreach (HexTile tile in neighboars)
            {
                if (movespeedLeft != 0 && !pathwalked.Contains(tile))
                {
                    int moveSpeedCost = (int)Math.Ceiling(tile.AStar.G / (float)100);
                    if (movespeedLeft >= moveSpeedCost)
                    {
                        FindMoveArea(ref result, ref pathwalked, tile, movespeedLeft - moveSpeedCost);
                    }
                }
            }
            pathwalked.RemoveLast(); // Remove itself from the list before returning.
        }
        #endregion

        #region Pathfinding
        private int GetHeuristic(HexTile node, HexTile end)
        {
            return (Math.Abs(node.GridIdx.X - end.GridIdx.X) + Math.Abs(node.GridIdx.Y - end.GridIdx.Y));
        }

        // pseudo: http://en.wikipedia.org/wiki/A*_search_algorithm
        /// <summary>
        /// Uses A-star algorithm. WORK IN PROGRESS! DOESNT WORK YET!
        /// reconstruct_path() needs work
        /// Also the BaseCost is not included now. But when I include it, it will also move backwards because those have a lower cost of course -.-
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The list of nodes or null if no route was found.</returns>
        public List<HexTile> GetRoute(HexTile start, HexTile end)
        {
            if (start == end)
                return new List<HexTile>() { end };

            List<HexTile> closedList = new List<HexTile>(); // The set of nodes already evaluated.
            List<HexTile> openList = new List<HexTile>() { start };    // The set of tentative nodes to be evaluated, initially containing the start node
            List<HexTile> cameFrom = new List<HexTile>();    // The map of navigated nodes.

            // F = G + H
            // Which square do we choose? The one with the lowest F cost.
            // G = the movement cost to move from the starting point A to a given square on the grid, following the path generated to get there. 
            // H = the estimated movement cost to move from that given square on the grid to the final destination, point B.
            //     This is often referred to as the heuristic, which can be a bit confusing. The reason why it is called that is
            //     because it is a guess. We really don’t know the actual distance until we find the path, because all sorts of
            //     things can be in the way (walls, water, etc.).

            start.AStar.G = 0; // G: Cost from start along best known path.
            start.AStar.H = GetHeuristic(start, end); // H: Heuristic. // F: Estimated total cost from start to goal through y.

            while (openList.Count > 0)
            {
                openList.Sort(new HexTileMoveCostComparer()); // Refactor note: this sorting every loop costs a lot of CPU. This can be optimized.
                HexTile currentNode = openList[0];

                if (currentNode.GridIdx == end.GridIdx) // If true then we found the ending node
                {
                    cameFrom.Add(end); // Add the desination itself also to the route.
                    return reconstruct_path(ref cameFrom, cameFrom.Last());
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                List<HexTile> neighboars = currentNode.GetPassableneigboars();

                foreach (HexTile neighboar in neighboars)
                {
                    if (closedList.Contains(neighboar))
                        continue;
                    int tentative_g_score = currentNode.AStar.G + neighboar.AStar.G; // tentative_g_score := g_score[x] + dist_between(x,y)
                    bool tentativeIsBetter = false;
                    if (!openList.Contains(neighboar))
                    {
                        openList.Add(neighboar);
                        tentativeIsBetter = true;
                    }
                    else if (tentative_g_score < neighboar.AStar.G)
                        tentativeIsBetter = true;

                    if (tentativeIsBetter)
                    {
                        cameFrom.Add(currentNode);
                        neighboar.AStar.G = tentative_g_score;
                        neighboar.AStar.H = GetHeuristic(neighboar, end);
                    }
                }
            }

            return null; // No possible route was found.
        }

        private List<HexTile> reconstruct_path(ref List<HexTile> cameFrom, HexTile currentNode)
        {
            return cameFrom;
            /*
                  if came_from[current_node] is set
        p = reconstruct_path(came_from, came_from[current_node])
        return (p + current_node)
    else
        return current_node
             */
        }


        #endregion

        #region Misc
        public void ClearSelection()
        {
            PreviousSelectedTiles = SelectedTiles;
            SelectedTiles = new List<HexTile>(); // Note: Do not use the .Clear() method as it will also delete the tiles on the PreviousSelectedTiles.
        }

        public HexTile TileAtPoint(Vector2 point)
        {
            // Return null if the point is not within the grid at all
            if (!m_GridArea.Contains(point))
                return null;

            point -= TopLeft;
            int x = (int)(point.X / TileWidth);
            int y = (int)(point.Y / (TileHeight * 0.75f));
            if (x >= ArrayWidth || y >= ArrayHeight)
                return null;

            return TileArray[x, y];
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Background
            if (BGTex != null)
                spriteBatch.Draw(BGTex, m_GridArea.ToRect().AddVector2(DrawOffset), Color.White);
            
            // Tiles
            Tiles.ForEach(t => t.Draw(spriteBatch, DrawOffset));
        }

        public void DrawCellCoordinates(SpriteBatch spriteBatch,Vector2 cameraOffset,  SpriteFont font, Color textColor)
        {
            Tiles.ForEach(t => t.DrawCellCoordinates(spriteBatch, cameraOffset, font, textColor));
        }

        public void DrawFGH(SpriteBatch spriteBatch, Vector2 cameraOffset, SpriteFont font, Color textColor)
        {
            Tiles.ForEach(t => t.DrawFGH(spriteBatch, cameraOffset, font, textColor));
        }
        #endregion
    }
}
