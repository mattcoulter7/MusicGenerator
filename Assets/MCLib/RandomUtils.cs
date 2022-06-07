using System;
using System.Collections;

namespace MCLib
{
    namespace MathUtils
    {
        public class RandomUtils
        {
            static System.Random rand = new System.Random(0);
            static public float RandomFloat(float min, float max)
            {
                double val = (rand.NextDouble() * (max - min) + min);
                return (float)val;
            }

            static public double RandomGaussian(double mean = 0, double stdDev = 1)
            {
                double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
                double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
                return randNormal;
            }

            static public double[] RandomGaussianArray(int samples,double mean = 0, double stdDev = 1){
                double[] result = new double[samples];
                for (int i = 0; i < samples; i++){
                    result[i] = RandomGaussian(mean,stdDev);
                }
                return result;
            }

            static public double[,] RandomGaussianArray2D(int samples,int samples2,double mean = 0, double stdDev = 1){
                double[,] result = new double[samples,samples2];
                for (int i = 0; i < samples; i++){
                    for (int j = 0; j < samples2; j++){
                        result[i,j] = RandomGaussian(mean,stdDev);
                    }
                }
                return result;
            }
        }
    }
}