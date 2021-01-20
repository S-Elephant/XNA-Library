using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System.Text;

namespace XNALib
{   
    /// <summary>
    /// Used internally for serializing and deserializing Dictionaries w/o using IXmlSerializable
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SerializeDictObject<TKey,TValue>
    {
        public List<TKey> Keys = new List<TKey>();
        public List<TValue> Values = new List<TValue>();
    }

    public static class Extensions
    {
        /// <summary>
        ///  Swaps characters in a string. Produces garbage.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public static string SwapCharacters(this string value, int position1, int position2)
        {
            char[] array = value.ToCharArray(); // Get characters
            char temp = array[position1]; // Get temporary copy of character
            array[position1] = array[position2]; // Assign element
            array[position2] = temp; // Assign element
            return new string(array); // Return string
        }

        /// <summary>
        ///  Swaps characters in a string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public static StringBuilder SwapCharacters(this StringBuilder value, int position1, int position2)
        {
            char c1 = value[position1];
            value[position1] = value[position2];
            value[position2] = c1;
            return value;
        }

        /// <summary>
        /// Clears the stringbuilder for the compact framework because .Clear() is missing there
        /// </summary>
        public static void ClearCompact(this StringBuilder sb)
        {
            sb.Length = 0;
            sb.Capacity = 0;
        }

#if WINDOWS
        /// <summary>
        /// Substracts ones list from another.
        /// NOT XBOX compatible.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToSubstractFrom">The original list that will have its items removed</param>
        /// <param name="itemsToSubstract">The list that contains the items that will be removed from the "listToSubstractFrom"</param>
        /// <returns></returns>
        public static List<T> SubstractList<T>(this List<T> listToSubstractFrom, List<T> itemsToSubstract)
        {
            HashSet<T> itemsToSubstractAsHashSet = new HashSet<T>(itemsToSubstract);
            listToSubstractFrom.RemoveAll(c => itemsToSubstractAsHashSet.Contains(c));
            return listToSubstractFrom;
        }

        /// <summary>
        /// Substracts ones list from another.
        /// NOT XBOX compatible.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToSubstractFrom">The original list that will have its items removed</param>
        /// <param name="itemsToSubstract">The HashSet that contains the items that will be removed from the "listToSubstractFrom"</param>
        /// <returns></returns>
        public static List<T> SubstractList<T>(this List<T> listToSubstractFrom, HashSet<T> itemsToSubstract)
        {
            listToSubstractFrom.RemoveAll(c => itemsToSubstract.Contains(c));
            return listToSubstractFrom;
        }
#endif

        public static List<int> ToIntList(this int number)
        {
            return number.ToIntList(1000000);
        }
        public static List<int> ToIntList(this int number, int initialDivider)
        {
            List<int> result = new List<int>();
            if (number == 0)
            {
                result.Add(0);
                return result;
            }

            number = Math.Abs(number);
            int divider = initialDivider;

            while (divider > number)
                divider = divider / 10;

            do
            {
                int temp = number / divider;
                result.Add(temp);
                number = number - (divider * temp);
                divider = divider / 10;
            } while (divider >= 1);

            return result;
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Point Add(this Point A, Point B)
        {
            return new Point(A.X + B.X, A.Y + B.Y);
        }

        public static FRect ToFRect(this Rectangle rect)
        {
            return new FRect(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public static Vector2 Origin(this Rectangle rect)
        {
            return new Vector2(rect.Left + rect.Width / 2, rect.Left + rect.Height / 2);
        }

#if WINDOWS
        /// <summary>
        /// DeSerializes a dictionary by converting it to two lists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="stream">An opened stream. The stream must also be manually closed</param>
        /// <returns>The deserialized dictionary</returns>
        public static Dictionary<TKey, TValue> DeSerialize<TKey, TValue>(this Dictionary<TKey, TValue> dict, ref StreamReader stream)
        {
            SerializeDictObject<TKey, TValue> serializeObject = new SerializeDictObject<TKey, TValue>();
            XmlSerializer serializer = new XmlSerializer(serializeObject.GetType());
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            serializeObject = (SerializeDictObject<TKey,TValue>)serializer.Deserialize(stream);
            
            for (int i = 0; i < serializeObject.Keys.Count; i++)
                result.Add(serializeObject.Keys[i], serializeObject.Values[i]);
            
            return result;
        }
#endif

        public static Vector2 Abs(this Vector2 value)
        {
            return new Vector2(Math.Abs(value.X), Math.Abs(value.Y));
        }

#if WINDOWS
        /// <summary>
        /// Serializes a dictionary by converting it to two lists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="stream">An opened stream. The stream must also be manually closed</param>
        public static void Serialize<TKey, TValue>(this Dictionary<TKey, TValue> dict, ref StreamWriter stream)
        {
            SerializeDictObject<TKey, TValue> serializeObject = new SerializeDictObject<TKey, TValue>();

            foreach (KeyValuePair<TKey,TValue> kvp in dict)
            {
                serializeObject.Keys.Add(kvp.Key);
                serializeObject.Values.Add(kvp.Value);
            }

            XmlSerializer serializer = new XmlSerializer(serializeObject.GetType());
            serializer.Serialize(stream, serializeObject);
        }
#endif

        public static T Last<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new ArgumentOutOfRangeException();

            return list[list.Count - 1];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks>Places the first item in the list on top of the stack.</remarks>
        public static Stack<T> ToStack<T>(this List<T> list)
        {
            Stack<T> result = new Stack<T>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                result.Push(list[i]);
            }
            return result;
        }

        /// <summary>
        /// http://blog.nickgravelyn.com/2008/12/how-to-test-if-a-player-can-purchase-your-game/
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool CanBuyGame(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            if (gamer == null)
                return false;

            return gamer.Privileges.AllowPurchaseContent;
        }

        public static void Push<T>(this List<T> list, T Value)
        {
            list.Add(Value);
        }

        public static T GetRandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new ArgumentOutOfRangeException();

            return list[Maths.RandomNr(0, list.Count - 1)];
        }

        public static T Pop<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new ArgumentOutOfRangeException();

            T result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            return result;
        }

        public static void Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        public static T Peek<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new ArgumentOutOfRangeException();

            return list[list.Count - 1];
        }

        public static void AddAttribute(this XElement xElement, string name, object value)
        {
            xElement.Add(new XAttribute(name, value.ToString()));
        }

        public static bool TrueForAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!predicate(list[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static XElement SelectChildElement(this XElement xElement, string name)
        {
            var query = xElement.Descendants(name).ToList();
            if (query.Count() > 0)
                return query.First();
            else
                return null;
        }

        public static XElement SelectSingleElement(this XElement xElement, string attrName, string attrValue)
        {
            var query = xElement.Descendants().Where(x => x.Attribute(attrName) != null && x.Attribute(attrName).Value == attrValue);
            if (query.Count() > 0)
                return query.Single();
            else
                return null;
        }

        public static Vector2 RoundDown(this Vector2 vector2)
        {
            return new Vector2((float)Math.Floor(vector2.X), (float)Math.Floor(vector2.Y));
        }

        public static Vector2 RoundUp(this Vector2 vector2)
        {
            return new Vector2((float)Math.Ceiling(vector2.X), (float)Math.Ceiling(vector2.Y));
        }

        /// <summary>
        /// Rounds the vectors coordinates to whole numbers.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector2 Round(this Vector2 vector2)
        {
            return new Vector2((float)Math.Round(vector2.X), (float)Math.Round(vector2.Y));
        }

        /// <summary>
        /// Because the compact framework does not have a RemoveAll with a predicate...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i--);
                }
            }
        }

        public static T Find<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return list[i];
                }
            }
            return default(T);
        }

        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static List<string> RemoveDuplicates(this List<string> inputList)
        {
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
            List<string> finalList = new List<string>();

            foreach (string currValue in inputList)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    finalList.Add(currValue);
                }
            }
            return finalList;
        }
        /*
        public static XmlNode NewNode(this XmlNode parentNode, ref XmlDocument doc, string name)
        {
            XmlNode newNode = doc.CreateElement(name);
            parentNode.AppendChild(newNode);
            return newNode;
        }

        public static XmlAttribute AddAttribute(this XmlNode node, ref XmlDocument doc, string name, object value)
        {
            return AddAttribute(node, ref doc, name, value.ToString());
        }
        public static XmlAttribute AddAttribute(this XmlNode node, ref XmlDocument doc, string name, string value)
        {
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = value;
            node.Attributes.Append(attr);
            return attr;
        }*/

        public static Rectangle AddPoint(this Rectangle rectangle, Point point)
        {
            return new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle AddXY(this Rectangle rectangle, int x, int y)
        {
            return new Rectangle(rectangle.X + x, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }

        public static Vector2 Inverse(this Vector2 vector)
        {
            return vector * new Vector2(-1, -1);
        }

        public static Rectangle AddVector2(this Rectangle rectangle, Vector2 vector)
        {
            return new Rectangle(rectangle.X + (int)vector.X, rectangle.Y + (int)vector.Y, rectangle.Width, rectangle.Height);
        }

        public static Vector2 ClampXY(this Vector2 vector, float minValueXY, float maxValueXY)
        {
            return new Vector2(MathHelper.Clamp(vector.X, minValueXY, maxValueXY), MathHelper.Clamp(vector.Y, minValueXY, maxValueXY));
        }

        /// <summary>
        /// Also includes the min and max values themselves.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="minValue"></param>
        /// <param name="maxMaxvalue"></param>
        /// <returns></returns>
        public static bool Between(this int value, int minValue, int maxMaxvalue)
        {
            return value >= minValue && value <= maxMaxvalue;
        }

        public static int Xi(this Vector2 vector)
        {
            return (int)vector.X;
        }
        public static int Yi(this Vector2 vector)
        {
            return (int)vector.Y;
        }

        public static Vector2 ToVector2(this Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }

        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Vector2 CenterVector(this Rectangle rect)
        {
            return rect.Center.ToVector2();
        }
    }
}
