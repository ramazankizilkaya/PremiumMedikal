using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Business.Helpers
{
    public static class CustomHelpers
    {
        private static Random rnd = new Random();
        public static HashSet<int> VisitedProducts = new HashSet<int>();


        public static void ShuffleMyList<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddVisitedProduct(int id)
        {
            VisitedProducts.Add(id);
        }

        public static void RemoveDuplicates<T>(this List<T> list)
        {
            HashSet<T> hashSet = new HashSet<T>();
            list.RemoveAll(x => !hashSet.Add(x));
        }
    }
}
