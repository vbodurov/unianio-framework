namespace Unianio.Extensions
{
    public static class BooleanExtensions
    {
        public static T ToValue<T>(this bool b, T ifTrue, T ifFalse) => b ? ifTrue : ifFalse;
    }
}