using System;
using System.Collections.Generic;

namespace WuxiaReader.Interface.Helpers
{
    public static class CollectionHelper
    {
        public static int BinarySearch<T>(IList<T> collection, T value, Comparer<T> comparer)
        {
            if (comparer == null) 
                throw new ArgumentException("Sorter must be supplied", nameof(comparer));

            var left = 0;
            var right = collection.Count - 1;

            while (left <= right)
            {
                var median = left + (right - left >> 1);
                var cmp = comparer.Compare(collection[median], value);
                
                if (cmp == 0)
                    return median;
                
                if (cmp < 0)
                    left = median + 1;
                else
                    right = median - 1;
            }
            
            return ~left;
        }
    }
}