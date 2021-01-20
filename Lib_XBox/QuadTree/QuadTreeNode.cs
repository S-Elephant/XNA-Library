using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public class QuadTreeNode<T>
        where T : ISpatial
    {
        // Cell
        public FRect AABB;

        // Childnodes
        public QuadTreeNode<T> TopLeft = null;
        public QuadTreeNode<T> TopRight = null;
        public QuadTreeNode<T> BottomRight = null;
        public QuadTreeNode<T> BottomLeft = null;

        // Misc
        public QuadTreeNode<T> ParentNode;
        public int MaxItems;
        public bool IsLeaf = true;
        public List<T> Items = new List<T>();

        // Depth of the node within the tree. The rootnode is depth 0.
        private int m_depth;
        public int Depth { get { return m_depth; } }

        // For debug drawing
        public Color BGColor = Color.Pink;

        #region Constructors
        /// <summary>
        /// Root node constructor
        /// </summary>
        /// <param name="AABB"></param>
        /// <param name="maxItems"></param>
        public QuadTreeNode(FRect AABB, int maxItems)
        {
            ParentNode = null; // Rootnode
            this.AABB = AABB;
            MaxItems = maxItems;
            m_depth = 0;
        }

        /// <summary>
        /// Regular constructor
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="AABB"></param>
        /// <param name="maxItems"></param>
        public QuadTreeNode(QuadTreeNode<T> parentNode, FRect AABB, int maxItems)
        {
            ParentNode = parentNode;
            this.AABB = AABB;
            MaxItems = maxItems;
            m_depth = parentNode.Depth + 1;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true when the item was added</returns>
        public bool AddItem(T item)
        {
            // Only add the item if it intersects with this node
            if (AABB.FullyEncloses(item.AABB))
            {
                if (IsLeaf)
                    SubDivide();

                // Try to add the item to a childnode but if no childnode will take it add it to this node
                if (!TopLeft.AddItem(item))
                    if (!TopRight.AddItem(item))
                        if (!BottomLeft.AddItem(item))
                            if (!BottomRight.AddItem(item))
                                Items.Add(item);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Works.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The node with the highest depth that contains the point or null if the point is outside the QuadTree.</returns>
        public QuadTreeNode<T> GetSmallestNode(ref Vector2 point)
        {
            if (AABB.Contains(point)) // Check if this node contains the point at all
            {
                if (!IsLeaf) // If it is divided then check if there is a sub
                {
                    QuadTreeNode<T> result = null;
                    result = TopLeft.GetSmallestNode(ref point);
                    if (result == null)
                    {
                        result = TopRight.GetSmallestNode(ref point);
                        if (result == null)
                        {
                            result = BottomRight.GetSmallestNode(ref point);
                            if (result == null)
                            {
                                result = BottomLeft.GetSmallestNode(ref point);
                                if (result == null)
                                    throw new Exception("Is not possible. If the node has the point then at least one of the 4 childnodes MUST have the point too.");
                            }
                        }
                    }
                    return result;
                }
                else
                    return this;
            }
            else
                return null;
        }

        public void ItemMoved(T item)
        {
            // What to do here? Loop trough all leaves and remove the item and then insert it again? This will cost too much CPU.
        }

        public void SubDivide()
        {
            if (!IsLeaf)
                throw new Exception("node is already divided.");
            IsLeaf = false;
            //Vector2 centerPoint = AABB.TopLeft + new Vector2(AABB.Width / 2, AABB.Height / 2);

            // Top left
            TopLeft = new QuadTreeNode<T>(this, new FRect(AABB.TopLeft, AABB.Width / 2, AABB.Height / 2), MaxItems);
            // Top right
            TopRight = new QuadTreeNode<T>(this, new FRect(AABB.TopLeft + new Vector2(AABB.Width / 2, 0), AABB.Width / 2, AABB.Height / 2), MaxItems);
            // Bottom left
            BottomRight = new QuadTreeNode<T>(this, new FRect(AABB.TopLeft + new Vector2(0, AABB.Height / 2), AABB.Width / 2, AABB.Height / 2), MaxItems);
            // Bottom right
            BottomLeft = new QuadTreeNode<T>(this, new FRect(AABB.TopLeft + new Vector2(AABB.Width / 2, AABB.Height / 2), AABB.Width / 2, AABB.Height / 2), MaxItems);
        }

        /// <summary>
        /// DOES NOT WORK. WIP. probably because the additems() doesnt work correctly.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="area"></param>
        /// <param name="skipChecks"></param>
        public void GetItemsWithinArea(ref List<T> items, ref FRect area, bool skipChecks)
        {
            if (skipChecks || AABB.Intersects(area))
            {
                // When the area if entirely within the Cell then skip collision checks on the child nodes
                if (area.FullyEncloses(AABB))
                    skipChecks = true;

                foreach (T item in items)
                {
                    if (skipChecks || item.AABB.Intersects(area))
                        items.Add(item);
                }

                if (!IsLeaf)
                {
                    TopLeft.GetItemsWithinArea(ref items, ref area, skipChecks);
                    TopRight.GetItemsWithinArea(ref items, ref area, skipChecks);
                    BottomLeft.GetItemsWithinArea(ref items, ref area, skipChecks);
                    BottomRight.GetItemsWithinArea(ref items, ref area, skipChecks);
                }
            }
        }

        /// <summary>
        /// Draws the node visually. Only used for debug purpose.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Common.White1px, AABB.ToRect(), BGColor);

            const int LINE_WIDTH = 3;
            spriteBatch.Draw(Common.White1px, new Rectangle((int)AABB.Left, (int)AABB.Top, (int)AABB.Width, LINE_WIDTH), Color.Red);
            spriteBatch.Draw(Common.White1px, new Rectangle((int)AABB.Right - LINE_WIDTH, (int)AABB.Top, LINE_WIDTH, (int)AABB.Height), Color.Red);
            spriteBatch.Draw(Common.White1px, new Rectangle((int)AABB.Left, (int)AABB.Bottom - LINE_WIDTH, (int)AABB.Width, LINE_WIDTH), Color.Red);
            spriteBatch.Draw(Common.White1px, new Rectangle((int)AABB.Left, (int)AABB.Top, LINE_WIDTH, (int)AABB.Height), Color.Red);

            if (!IsLeaf)
            {
                TopLeft.Draw(spriteBatch);
                TopRight.Draw(spriteBatch);
                BottomRight.Draw(spriteBatch);
                BottomLeft.Draw(spriteBatch);
            }
        }
    }
}
