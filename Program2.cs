using System.Diagnostics;
using System.Linq;
using static System.Math;
static partial class Program
{
    static bool _stop;
    static void Main()
    {
        try
        {
            var neuralNetwork = new NeuralNetwork("5,4", ActivateFunction.Lin);
            EvolutionProcess process = new EvolutionProcess(Evolution.GetNeuralNetwork(neuralNetwork, ActivateFunction.Lin, -5, 5), IsGoodSystem, 0.3, new List<NeuralNetwork>(20));
            while (!_stop)
            {
                DrawField();
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
        }
        finally
        {
            Console.ReadKey();
        }
    }
}