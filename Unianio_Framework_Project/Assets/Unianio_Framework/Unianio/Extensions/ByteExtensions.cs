namespace Unianio.Extensions
{
    public static class ByteExtensions
    {
        public static bool MaskContains(this byte mask, byte value)
        {
            return (mask & value) > 0;
        }
        public static bool MaskDoesNotContain(this byte mask, byte value)
        {
            return (mask & value) == 0;
        }
        public static ulong AddToMask(ref this byte mask, byte value)
        {
            mask |= value;
            return mask;
        }
        public static ulong RemoveFromMask(ref this byte mask, byte value)
        {
            int maskInt = mask;
            int valueInt = value;
            maskInt &= ~valueInt;
            return (byte)maskInt;
        }
    }
}