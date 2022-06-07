using Newtonsoft.Json;
namespace MCLib
{
    namespace MathUtils
    {
        public static class MatrixUtils
        {
            static void ValidateSameSize(double[,] A, double[,] B)
            {
                int w1 = A.GetLength(0);
                int h1 = A.GetLength(1);
                int w2 = B.GetLength(0);
                int h2 = B.GetLength(1);
                if (h1 != w2)
                {
                    throw new System.Exception("Matrices cannot be multiplied");
                }
            }

            public static double[,] Multiply(double[,] A, double[,] B)
            {
                ValidateSameSize(A,B);
                int w1 = A.GetLength(0);
                int h1 = A.GetLength(1);
                int h2 = B.GetLength(1);
                double temp = 0;
                double[,] result = new double[w1, h2];
                
                for (int i = 0; i < w1; i++)
                {
                    for (int j = 0; j < h2; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < h1; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        result[i, j] = temp;
                    }
                }
                return result;
            }

            public static double[,] Transpose(this double[,] M)
            {
                int w = M.GetLength(0);
                int h = M.GetLength(1);

                double[,] result = new double[h, w];

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        result[j, i] = M[i, j];
                    }
                }

                return result;
            }

            public static double[,] Add(double[,] A, double[,] B)
            {
                ValidateSameSize(A, B);

                int w = A.GetLength(0);
                int h = B.GetLength(1);

                double[,] result = new double[w, h];

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        result[i, j] = A[i, j] + B[i, j];
                    }
                }

                return result;
            }

            public static double[,] SumVectorToMatrix(double[,] M, double[] V)
            {
                int w = M.GetLength(0);
                int h = M.GetLength(1);

                double[,] result = new double[w, h];

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        result[i, j] = M[i, j] + V[j];
                    }
                }

                return result;
            }

            public static string Print(object obj)
            {
                return $@"{{{JsonConvert.SerializeObject(obj)
                    .Trim('[', ']').Replace("[", "{").Replace("]", "}")}}}";
            }
        }
    }
}