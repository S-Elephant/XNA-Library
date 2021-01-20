using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public static class Common
    {
        public static Texture2D White1px { get { return str2Tex("white1px"); } }
        public static Texture2D White1px50Trans { get { return str2Tex("white1px50Trans"); } }
        public static Texture2D Black1px { get { return str2Tex("black1px"); } }
        public static Texture2D Black1px50Trans { get { return str2Tex("black1px50Trans"); } }
        public static SpriteFont DefaultFont { get { return str2Font("Default"); } }
        public static string MeasureString { get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; } }
        public static Vector2 InvalidVector2 { get { return new Vector2(float.MinValue, float.MinValue); } }
        public static Point InvalidPoint { get { return new Point(int.MinValue, int.MinValue); } }

        public static readonly string[] StrNumbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        #region conversions to and from string
        public static Texture2D str2Tex(string texture)
        {
            return Global.Content.Load<Texture2D>(Global.TexturesFolder + texture);
        }
        public static Texture2D str2Tex(params string[] textures)
        {
            return Global.Content.Load<Texture2D>(Global.TexturesFolder + textures[Maths.RandomNr(0, textures.GetLength(0) - 1)]);
        }
        public static SpriteFont str2Font(string font)
        {
            return Global.Content.Load<SpriteFont>(Global.FontFolder + font);
        }
        public static Vector2 Str2Vector(string str)
        {
            string[] temp = str.Split(';');
            return new Vector2(float.Parse(temp[0]),float.Parse(temp[1]));
        }
        public static Point Str2Point(string str)
        {
            string[] temp = str.Split(';');
            return new Point(int.Parse(temp[0]), int.Parse(temp[1]));
        }
        public static string Point2Str(Point point)
        {
            return string.Format("{0};{1}", point.X, point.Y);
        }
        public static string Vector2Str(Vector2 vector2)
        {
            return string.Format("{0};{1}", vector2.X, vector2.Y);
        }
        public static Model str2Model(string model)
        {
            return Global.Content.Load<Model>(Global.ModelFolder + model);
        }
        public static string Rectangle2Str(Rectangle rectangle)
        {
            return string.Format("{0};{1};{2};{3}", rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public static Rectangle Str2Rectangle(string str)
        {
            string[] temp = str.Split(';');
            return new Rectangle(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]), int.Parse(temp[3]));
        }
        #endregion

        public static Vector2 CenterStringX(SpriteFont font, string str, int screenWidth, float y)
        {
            return new Vector2(screenWidth / 2 - font.MeasureString(str).X / 2, y);
        }
        public static Vector2 CenterStringY(SpriteFont font, string str, float x,int screenHeight)
        {
            return new Vector2(x, screenHeight / 2 - font.MeasureString(str).Y / 2);
        }
        public static Vector2 CenterString(SpriteFont font, string str, int screenWidth, int screenHeight)
        {
            return new Vector2(screenWidth / 2 - font.MeasureString(str).X / 2, screenHeight / 2 - font.MeasureString(str).Y / 2);
        }
        public static Vector2 CenterStringOnPoint(SpriteFont font, string str, Vector2 point)
        {
            Vector2 measure = font.MeasureString(str);
            return new Vector2(point.X - measure.X / 2, point.Y - measure.Y / 2);
        }
    }
}
