/*
 * TODO:
 * - Push items to child nodes when the # of items > maxitems per node.
 * - What do I do with a moving/scaling item?
 * - How do I handle a big item that spans across multiple nodes?
 * - Return the items that are inside a certain rectangle.
 * - What happens when 3 or more objects of the same size are located at the same position. Will the QuadTree subdivide forever? As there will always be > 2 items/cell for those.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNALib
{
    #warning Work In Progress: DO NOT USE YET!
    public class QuadTree<T>
        where T : ISpatial
    {
        #region Properties
        float WorldWidth, WorldHeight;
        public QuadTreeNode<T> RootNode;
        int MaxItems = 2;
        #endregion

        public QuadTree(float width, float height)
        {
            WorldWidth = width;
            WorldHeight = height;

            RootNode = new QuadTreeNode<T>(new FRect(0, 0, width, height), MaxItems);
        }

        /// <summary>
        /// Returns all items (not nodes) that intersect with the given area.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<T> GetItemsWithinArea(FRect area)
        {
            List<T> items = new List<T>();
            RootNode.GetItemsWithinArea(ref items, ref area, false);
            return items;
        }

        /// <summary>
        /// Returns the node with the largest depth that intersects with the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public QuadTreeNode<T> GetSmallestNode(Vector2 point)
        {
            return RootNode.GetSmallestNode(ref point);
        }

        public void ItemMoved(T item)
        {
            RootNode.ItemMoved(item);
        }

        /// <summary>
        /// Adds a new item to the QuadTree
        /// </summary>
        /// <param name="item"></param>
        public void InsertItem(T item)
        {
            RootNode.AddItem(item);
        }

        /// <summary>
        /// Debug draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawNodes(SpriteBatch spriteBatch)
        {
            RootNode.Draw(spriteBatch);
        }
    }
}
