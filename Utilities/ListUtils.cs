using System.Collections.Generic;

namespace SpiritMod.Utilities
{
    public static class ListUtils
    {
        public static void AddWithCondition<T>(this List<T> list, T item, bool condition) {
            if(condition) {
                list.Add(item);
            }
        }
    }
}
