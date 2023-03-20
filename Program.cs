// using System.Diagnostics;
// using System.Linq;
// using static System.Math;
// static partial class Program
// {
//     static float[] _weightsSymbols = new float[256];//127
//     static string[] _data = new string[] { };
//     static string[] _data2 = new string[] { };
//     static float[] _weights = new float[_data.Length];
//     static float[] _weights2 = new float[_data.Length];
//     private static bool _diffWeightsUse;
//     static float[] _prevWeights = new float[_data.Length];
//     static float[] _diffWeights = new float[_data.Length];
//     static float _unconnect;
//     static float _unconnectPrev;
//     private static float _unconnect2Prev;
//     private static Random _rng = new Random(1);
//     private static bool _end;
//     private static int _iterator = 1000000;
//     private static float _unconnect2;
//     private static float[] _prevWeights2;
//     private static float[] _diffWeights2;

//     static void Main()
//     {
//         try
//         {
//             _field[0, 0] = Cell.Player;
//             _field[19, 19] = Cell.Exit;
//             Console.CursorVisible = false;
//             if (_end || _iterator == 0)
//             {
//                 return;
//             }
//             OnRandom(0.1, Cell.Projectile);
//             _weights = new float[_data.Length];
//             _weights2 = new float[_data.Length];
//             _prevWeights = new float[_data.Length];
//             _diffWeights = new float[_data.Length];
//             _prevWeights2 = new float[_data.Length];
//             _diffWeights2 = new float[_data.Length];
//             _data = new string[] { };
//             _data2 = new string[] { };
//             //Read("File.txt", 24, true);
//             //Read("File2.txt", 5, false);
//             // for (int i = 0; i < 1500; i++)
//             // {
//             //     Tick();
//             // }
//             while (true)
//             {
//                 DrawField();
//             }
//         }
//         catch (Exception exception)
//         {
//             Console.Error.WriteLine(exception);
//         }
//         finally
//         {
//             Console.ReadKey();
//         }
//     }

//     // static void Tick()
//     // {
//     //     _weightsSymbols[_rng.Next(_weightsSymbols.Length)] += _rng.Next(-100, 101);
//     //     _weightsSymbols[_rng.Next(_weightsSymbols.Length)] += _rng.Next(-100, 101);
//     //     _prevWeights = _weights;
//     //     _diffWeights = _weights.Zip(_prevWeights, (float a, float b) => Abs(a - b)).ToArray();//?
//     //     _diffWeights2 = _weights2.Zip(_prevWeights2, (float a, float b) => Abs(a - b)).ToArray();//?
//     //     //var i = (float)(((MathCollection)_weights).Difference().Average());//?
//     //     //var j = (float)(((MathCollection)_weights2).Difference().Average());//?
//     //     _unconnectPrev = _unconnect;
//     //     _unconnect2Prev = _unconnect2;
//     //     _unconnect = UnconnectValue(_data);
//     //     _unconnect2 = UnconnectValue(_data2);
//     //     if (_unconnect is > 30 and < 100 && _unconnect2 > 150)
//     //     {
//     //     }
//     //     if (_unconnect - _unconnectPrev > 3)
//     //     {
//     //         _weights = _prevWeights;
//     //     }
//     //     else if (_unconnect - _unconnectPrev < -3 || _diffWeightsUse)
//     //     {
//     //         _weights = _weights.Zip(_diffWeights, (float a, float b) => a + b).ToArray();
//     //         _diffWeightsUse = true;
//     //         return;
//     //     }
//     //     if (_unconnect2 - _unconnect2Prev > -3)
//     //     {
//     //         _weights2 = _prevWeights2;
//     //     }
//     //     else if (_unconnect2- _unconnect2Prev < 3 || _diffWeightsUse)
//     //     {
//     //         _weights2 = _weights2.Zip(_diffWeights2, (float a, float b) => a + b).ToArray();
//     //         _diffWeightsUse = true;
//     //         return;
//     //     }
//     // }
//     // static float UnconnectValue(string[] s)
//     // {
//     //     _weights = s.Select(Weight).ToArray();
//     //     var i = (int)(((MathCollection)_weights).Difference().Average());//?
//     //     _unconnect = i;
//     //     return _unconnect;
//     // }
//     // static float UnconnectValue(string s)
//     // {
//     //     return UnconnectValue(s.Split(' '));
//     // }
//     // static void Read(string file, int length, bool isGoodData)
//     // {
//     //     List<string> strings = new List<string>();
//     //     var reader = File.OpenText(file);
//     //     for (int i = 0; i < length; i++)
//     //     {
//     //         strings.AddRange(reader.ReadLine()!.Split(' '));
//     //     }
//     //     if (isGoodData)
//     //     {
//     //         _data = strings.ToArray();
//     //     }
//     //     else
//     //     {
//     //         _data2 = strings.ToArray();
//     //     }
//     // }
//     // static float Weight(string s)
//     // {
//     //     return s.Sum(c => _weightsSymbols[c]);
//     // }
//     static void Random(float[] prob)//?
//     {
//         var next = _rng.Next();
//         var sum = prob.Aggregate((float a, float b) => a + b);
//         var probs = prob.Select((float a) => a / sum).ToArray();
//         probs.Select((float d, int i) => { if (next < probs[1..i].Sum()) { return i; } return 0; });
//     }
//     // static float DisperseHandle(float d)
//     // {
//     //     return d + 1 / (d + 1 / 100);
//     // }
// }