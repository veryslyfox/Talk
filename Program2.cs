using System.Diagnostics;
using System.Linq;
using static System.Math;
static partial class Program
{
    static bool _stop;
    private static long _moveTime;
    //     System.NullReferenceException: Object reference not set to an instance of an object.
    //    at Neuron.WriteValue() in C:\Users\Лисёнок\Documents\Talk\NeuralNetwork.cs:line 99
    //    at NeuralNetwork.Use(Single[] data) in C:\Users\Лисёнок\Documents\Talk\NeuralNetwork.cs:line 19
    //    at Program.GetResolution(NeuralNetwork network) in C:\Users\Лисёнок\Documents\Talk\Simulation.cs:line 237
    //    at Program.Main() in C:\Users\Лисёнок\Documents\Talk\Program2.cs:line 51
    static void Main()
    {
        try
        {
            Error.Write();
            var prob = 0;
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                for (int row = 0; row < _field.GetLength(1); row++)
                {
                    Error.Write();
                    if (column == 0 || column == _field.GetLength(0) - 1 || row == 0 || row == _field.GetLength(1) - 1)
                    {
                        _field[column, row] = Cell.Barrier;
                    }
                    else
                    {
                        _field[column, row] = _rng.NextDouble() > prob ? Cell.Empty : Cell.Projectile;
                    }
                    Error.Write();
                }
            }
            Error.Write();
            _field[20, 20] = Cell.Exit;
            _playerX = 1;
            _playerY = 1;
            var neuralNetwork = new NeuralNetwork("4,4", ActivateFunction.Lin);
            var array = new NeuralNetwork[20];
            Array.Fill(array, Evolution.GetNeuralNetwork(neuralNetwork, ActivateFunction.Lin, -5, 5));
            EvolutionProcess process = new EvolutionProcess(Evolution.GetNeuralNetwork(neuralNetwork, ActivateFunction.Lin, -5, 5), IsGoodSystem, 0.3F, new List<NeuralNetwork>(array));
            Console.CursorVisible = false;
            var i = 0;
            Error.Write();
            while (!_stop)
            {
                i++;
                process.Selection(100);
                DrawField();
                if (i == 40)
                {
                    _stop = true;
                }
                _field[1, 1] = Cell.Player;
            }
            Error.Write();
            for (int j = 0; j < 200; j++)
            {
                DrawField();
                TimeSync();
                GetResolution(process.Networks[0]);
                TimeSync();
                _moveTime = _time;
            }
            Error.Write();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
        }
        finally
        {
            Console.ReadKey();
        }
        static void TimeSync()
        {
            _time = (long)(Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency * 1000);
        }
    }
}