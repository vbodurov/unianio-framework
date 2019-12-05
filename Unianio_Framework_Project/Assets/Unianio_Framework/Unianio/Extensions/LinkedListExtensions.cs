using System.Collections.Generic;

namespace Unianio.Extensions
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<LinkedListNode<T>> ToNodes<T>(this LinkedList<T> list)
        {
            var node = list.First;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }
        public static void AddRangeAsLast<T>(this LinkedList<T> list, IEnumerable<T> toAdd)
        {
            if(toAdd == null) return;
            foreach (var e in toAdd)
            {
                list.AddLast(e);
            }
        }
        public static LinkedListNode<T> GetNodeByIndex<T>(this LinkedList<T> list, int index)
        {
            if (list == null || index < 0 || index >= list.Count) return null;
            var node = list.First;
            var i = 0;
            while (node != null)
            {
                if (i == index) return node;
                ++i;
                node = node.Next;
            }
            return null;
        }
    }
}