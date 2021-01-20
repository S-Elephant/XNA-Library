
namespace XNALib.RectSideExtensions
{
    public static class RectSideExtensions
    {
        public static bool IsSet(this Collision.RectSide side, Collision.RectSide flags)
        {
            return (side & flags) == flags;
        }

        public static bool IsNotSet(this Collision.RectSide side, Collision.RectSide flags)
        {
            return (side & (~flags)) == 0;
        }

        public static Collision.RectSide Set(this Collision.RectSide side, Collision.RectSide flags)
        {
            return side | flags;
        }

        public static Collision.RectSide Clear(this Collision.RectSide side, Collision.RectSide flags)
        {
            return side & (~flags);
        }
    }
}
