using Newtonsoft.Json;
namespace MCLib
{
    namespace MathUtils
    {
        public static class ArrayUtils
        {
            public static void Fill<T>(this T[] originalArray, T with)
            {
                for (int i = 0; i < originalArray.Length; i++)
                {
                    originalArray[i] = with;
                }
            }
        }
    }
}