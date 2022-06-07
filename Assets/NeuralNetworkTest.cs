using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MCLib.MachineLearning.NeuralNetwork;
using MCLib.MathUtils;

using MathNet.Numerics.LinearAlgebra;

public class NeuralNetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Matrix<double> inputs = Matrix<double>.Build.DenseOfArray(new double[,] {{ 0, 2, -1, 3.3 }});
        (Matrix<double>,Vector<double>) data = CreateData.Run(100,3);

        Layer_Dense dense1 = new Layer_Dense(2, 3);
        Activation_ReLU activation1 = new Activation_ReLU();

        Layer_Dense dense2 = new Layer_Dense(3, 3);
        Activation_SoftMax activation2 = new Activation_SoftMax();

        Loss_CategoricalCrossEntropy lossFunction = new Loss_CategoricalCrossEntropy();


        dense1.Forward(data.Item1);
        activation1.Forward(dense1.output);
        dense2.Forward(activation1.output);
        activation2.Forward(dense2.output);

        double loss = lossFunction.Calculate(activation2.output,data.Item2);

        Debug.Log(loss);
    }
}
