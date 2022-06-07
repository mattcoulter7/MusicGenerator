using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace MCLib
{
    namespace MachineLearning
    {
        namespace NeuralNetwork
        {

            public abstract class Outputter
            {
                protected Matrix<double> _output;
                public Matrix<double> output
                {
                    get
                    {
                        return _output;
                    }
                }
                public Outputter() { }

                public abstract void Forward(Matrix<double> inputs);
            }
            public class Activation_ReLU : Outputter
            {
                public Activation_ReLU() : base() { }
                public override void Forward(Matrix<double> inputs)
                {
                    _output = inputs.PointwiseMaximum(0);
                }
            }
            public class Activation_SoftMax : Outputter
            {
                public Activation_SoftMax() : base() { }
                public override void Forward(Matrix<double> inputs)
                {
                    // calculate the maximum of each row vector
                    Vector<double> maximums = Vector<double>.Build.Dense(inputs.RowCount);
                    foreach ((int,Vector<double>) item in inputs.EnumerateRowsIndexed())
                    {
                        maximums[item.Item1] = item.Item2.Maximum();
                    }

                    // to avoid values too high from exponentials, we can subtract the maximum value in each row vector
                    inputs.MapIndexedInplace((int rowIndex,int columnIndex,double value) =>
                    {
                        return value - maximums[rowIndex];
                    });
                    
                    // calculate the exponentials
                    _output = inputs.PointwiseExp();

                    // normalise the rows
                    _output = _output.NormalizeRows(1);
                }
            }
            public class Layer_Dense : Outputter
            {
                private Matrix<double> weights;
                private Matrix<double> biases;
                public Layer_Dense(int numInputs, int numNeurons) : base()
                {
                    //creats shortcuts to Matrix and Vector builders: 
                    var M = Matrix<double>.Build;
                    var V = Vector<double>.Build;

                    //creates a n_inputs x n_neurons random matrix and assign to weights
                    weights = M.Random(numInputs, numNeurons);

                    //creates a zero filled vector, this has to be a horizontal vector, hense using a 1xn matrix
                    biases = M.Dense(1, numNeurons);
                }

                public override void Forward(Matrix<double> inputs)
                {
                    //creates a Matrix of size: (batches x 1) , this is just an aux "vector"
                    var M = Matrix<double>.Build;
                    var v = M.Dense(inputs.RowCount, 1, 1);
                    //multiply matrix v*biases: (batches x 1) dot (1 x neurons) = (batches x neurons) where each row is the same as the horizontal biases vector
                    var biasm = v * biases; //biasm is a matrix where each row is identical and the rows are the biases horizontal vector
                                            //now bias matrix can be added to inputs*weights
                    _output = inputs * weights + biasm;
                }
            }
            public abstract class Loss
            {
                public Loss() { }
                public abstract Vector<double> Forward(Matrix<double> output, Vector<double> y);
                public double Calculate(Matrix<double> output, Vector<double> y)
                {
                    Vector<double> sampleLosses = Forward(output, y);
                    //double sampleMean = sampleLosses.Norm(sampleLosses.Sum());
                    double sampleMean = sampleLosses.Sum() / sampleLosses.Count;
                    return sampleMean;
                }
            }

            public class Loss_CategoricalCrossEntropy : Loss
            {
                public Loss_CategoricalCrossEntropy() : base() { }
                public override Vector<double> Forward(Matrix<double> output, Vector<double> y)
                {
                    // clip all the value by an insiginificant number to avoid infinite result
                    double clipMin = Math.Pow(10, -7);
                    double clipMax = 1 - clipMin;
                    Matrix<double> clippedOutput = Matrix<double>.Build.SameAs(output);
                    output.Map((double value) =>
                    {
                        // manual clamp operation
                        value = Math.Min(value, clipMax);
                        value = Math.Max(value, clipMin);
                        return value;
                    }, clippedOutput);

                    // extract the chosen values from the output set (index mapping)
                    Vector<double> correctConfidences = Vector<double>.Build.SameAs(y);
                    y.MapIndexed((int index,double value) =>
                    {
                        return output[index,(int)value];
                    }, correctConfidences);

                    // calculate negative natural logs for each value
                    Vector<double> negativeLogLikelihoods = -correctConfidences.PointwiseLog();
                    return negativeLogLikelihoods;
                }

            }



            public static class CreateData
            {
                public static (Matrix<double>,Vector<double>) Run(int points, int classes)
                {
                    //Matrix<double> X;
                    //Vector<double> y;
                    var M = Matrix<double>.Build; //shortcut to Matrix builder
                    var V = Vector<double>.Build; //shortcut to Vector builder

                    //build vectors of size points*classesx1 for y, r and theta
                    var y = V.Dense(points * classes); //at this point this is full of zeros
                    for (int j = 0; j < classes; j++)
                    {
                        var y_step = V.DenseOfArray(Generate.Step(points * classes, 1, (j + 1) * points));
                        y = y + y_step;
                    }
                    var r = V.DenseOfArray(Generate.Sawtooth(points * classes, points, 0, 1));
                    var theta = 4 * (r + y) + (V.DenseOfArray(Generate.Standard(points * classes)) * 0.2);
                    var sin_theta = theta.PointwiseSin();
                    var cos_theta = theta.PointwiseCos();

                    var X = M.DenseOfColumnVectors(r.PointwiseMultiply(sin_theta), r.PointwiseMultiply(cos_theta));
                    return (X,y);
                }
            }
        }
    }
}