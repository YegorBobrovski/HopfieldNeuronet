using System.Collections.Generic;
using System.Security.Cryptography;

namespace NeuroNet
{
    public static class Utils
    {
        private static readonly RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider();

        public static IEnumerable<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do
                    CryptoProvider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                int k = box[0] % n;
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static IEnumerable<double> Range(double min, double step, double max)
        {
            for (double i = min; i < max + 0.99*step; i += step)
                yield return i;
        } 
    }
}
