using Microsoft.Xna.Framework.Content;
namespace XNALib
{
    public static class Global
    {
        private static ContentManager m_content = null;
        public static ContentManager Content
        {
            get { return GetContentMgr(); }
            set { m_content = value; }
        }
        private static string m_TexturesFolder = "Textures\\";
        public static string TexturesFolder
        {
            get { return Global.m_TexturesFolder; }
            set { Global.m_TexturesFolder = value; }
        }
        private static string m_FontFolder = "Fonts\\";
        public static string FontFolder
        {
            get { return Global.m_FontFolder; }
            set { Global.m_FontFolder = value; }
        }
        private static string m_EffectFolder = "Effects\\";
        public static string EffectFolder
        {
            get { return Global.m_EffectFolder; }
            set { Global.m_EffectFolder = value; }
        }
        private static string m_ModelFolder = "Models\\";
        public static string ModelFolder
        {
            get { return Global.m_ModelFolder; }
            set { Global.m_ModelFolder = value; }
        }

        private static ContentManager GetContentMgr()
        {
            return m_content;
        }
    }
}
