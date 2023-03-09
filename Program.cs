using System.Diagnostics;
using System.Linq;
using static System.Math;
static class Program
{
    static int[] _weightsSymbols = new int[65536];//127
    static string[] _data = new string[] { };
    static int[] _weights = new int[_data.Length];
    static int[] _prevWeights = new int[_data.Length];
    static int[] _diffWeights = new int[_data.Length];
    static double _unconnect;
    static double _unconnectPrev;
    private static Random _rng = new Random(1);
    private static bool _end;
    private static int _iterator = 10000;
    static void Main()
    {
        try
        {
            if(_iterator == 0)
            {
                return;
            }
            Read("File.txt", 30);
            _weights = new int[_data.Length];
            _prevWeights = new int[_data.Length];
            _diffWeights = new int[_data.Length];
            Array.Clear(_weights);
            Array.Clear(_prevWeights);
            Array.Clear(_diffWeights);
            for (int i = 0; i < 1000; i++)
            {
                Tick();
            }
            _iterator--;
            Console.WriteLine(UnconnectValue(new string[] { "abcabcabcabcabcabcabc" }));
            Main();
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
    static void Tick()
    {
        _weightsSymbols[_rng.Next(_weightsSymbols.Length)] += _rng.Next(-3, 4);
        _weightsSymbols[_rng.Next(_weightsSymbols.Length)] += _rng.Next(-3, 4);
        _prevWeights = _weights;
        _weights = _data.Select((string a, int b) => Weight(_data[b]) / a.Length).ToArray();
        _diffWeights = _weights.Zip(_prevWeights, (int a, int b) => Abs(a - b)).ToArray();//?
        var i = (int)(((MathCollection)_weights).Difference().Average());//?
        _unconnectPrev = _unconnect;
        _unconnect = DisperseHandle(i);
        if (_unconnect - _unconnectPrev > 3)
        {
            _weights = _prevWeights;
        }
        else
        {
            _end = false;
            return;
        }
        if (_unconnect > 2)
        {
            _weights = _weights.Zip(_diffWeights, (int a, int b) => a + b).ToArray();
            return;
        }
    }
    static double UnconnectValue(string[] s)
    {
        _weights = s.Select(Weight).ToArray();
        var i = (int)(((MathCollection)_weights).Difference().Average());//?
        Console.WriteLine(i);
        _unconnect = DisperseHandle(i);
        return _unconnect;
    }
    static double UnconnectValue(string s)
    {
        return UnconnectValue(s.Split(' '));
    }
    static void Read(string file, int length)
    {
        List<string> strings = new List<string>();
        var reader = File.OpenText(file);
        for (int i = 0; i < length; i++)
        {
            strings.AddRange(reader.ReadLine()!.Split(' '));
        }
        _data = strings.ToArray();
    }
    static int Weight(string s)
    {
        return s.Sum(c => { return _weightsSymbols[c]; });
    }
    static void Random(string[] strings, double[] prob)//?
    {
        var next = _rng.Next();
        var sum = prob.Aggregate((double a, double b) => a + b);
        var probs = prob.Select((double a) => a / sum).ToArray();
        probs.Select((double d, int i) => { if (next < probs[1..i].Sum()) { return strings[i]; } return ""; });
    }
    static double DisperseHandle(double d)
    {
        return d + 1 / (d + 1 / 30);
    }
}