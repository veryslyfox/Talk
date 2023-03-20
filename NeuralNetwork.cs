using static System.MathF;
enum ActivateFunction
{
    Exp,
    Tanh,
    LRect,
    Atan,
    Lin
}
class NeuralNetwork
{
    public NeuralNetwork(NeuronLayer[] neuronLayers, ActivateFunction function)
    {
        NeuronLayers = neuronLayers;
        Function = function;
    }
    public NeuralNetwork(string arh, ActivateFunction function)
    {
        NeuronLayers = arh.Split(',').Select(s => new NeuronLayer(new(int.Parse(s)), null)).ToArray();
        Function = function;
    }
    public NeuronLayer this[int index]
    {
        get => NeuronLayers![index];
    }
    public NeuronLayer[]? NeuronLayers { get; set; }
    public ActivateFunction Function { get; }
}

class Neuron
{
    static float Activate(float f, ActivateFunction activate)
    {
        switch (activate)
        {
            case ActivateFunction.Exp:
                return 1 + 1 / Exp(-f);
            case ActivateFunction.Tanh:
                return Tanh(f);
            case ActivateFunction.LRect:
                return Max(0, f);
            case ActivateFunction.Atan:
                return 2 * Atan(f) / PI;
            case ActivateFunction.Lin:
                return f;
        }
        return f;
    }
    static float Arctivate(float f, ActivateFunction activate)
    {
        switch (activate)
        {
            case ActivateFunction.Exp:
                return 1 + 1 / Exp(-f);
            case ActivateFunction.Tanh:
                return Atanh(f);
            case ActivateFunction.LRect:
                return Max(0, f);
            case ActivateFunction.Atan:
                return Tan(f);
            case ActivateFunction.Lin:
                return f;
        }
        return f;
    }
    public Neuron(float[] weights, NeuronLayer layer, ActivateFunction activateFunction)
    {
        Weights = weights;
        Layer = layer;
        ActivateFunction = activateFunction;
        layer.Neurons.Add(this);
        WriteValue();
    }
    public void WriteValue()
    {
        Value = Activate(Weights.Zip(Layer.Neurons, (float i, Neuron neuron) => i * neuron.Value).Sum(), ActivateFunction);
    }
    public float Value { get; set; }
    public float[] Weights { get; set; }
    public NeuronLayer Layer { get; set; }
    public ActivateFunction ActivateFunction { get; }
}
class NeuronLayer
{
    public NeuronLayer(List<Neuron> neurons, NeuronLayer? next)
    {
        Neurons = neurons;
        Next = next;
    }

    public List<Neuron> Neurons { get; }
    public NeuronLayer? Next { get; set; }
    public NeuronLayer? Previous { get; set; }
}
enum Cell
{
    Empty,
    Player,
    Barrier,
    Exit,
    Projectile,
    OffsetedProjectile,
    Cannon,
    Never,
    Bomb,
}


static class Evolution
{
    static Random _rng = new Random();
    public static NeuronLayer RandomLayer(float min, float max, NeuronLayer layer, ActivateFunction function)
    {
        for (int i = 0; i < layer.Neurons.Count; i++)
        {
            new Neuron(new MathCollection(i => _rng.NextSingle() * (max - min) + min, layer.Neurons.Count), layer, function);
        }
        return layer;
    }
    public static NeuronLayer RandomLayer(NeuronLayer layer, NeuronLayer mean, ActivateFunction function, float disperse)
    {
        for (int i = 0; i < layer.Neurons.Count; i++)
        {
            new Neuron(new MathCollection(i => _rng.NextSingle() * disperse * 2 + mean.Neurons[i].Value - disperse, layer.Neurons.Count), layer, function);
        }
        return layer;
    }
    public static NeuralNetwork GetNeuralNetwork(NeuralNetwork network, ActivateFunction function, float min, float max)
    {
        for (int i = 0; i < network.NeuronLayers!.Length; i++)
        {
            var item = network.NeuronLayers[i];
            network.NeuronLayers[i] = RandomLayer(min, max, item, function);
        }
        return network;
    }
    public static NeuralNetwork GetNeuralNetwork(NeuralNetwork mean, float disperse, NeuralNetwork buffer)
    {
        for (int i = 0; i < mean.NeuronLayers!.Length; i++)
        {
            var item = mean.NeuronLayers[i];
            mean.NeuronLayers[i] = RandomLayer(buffer[i], mean[i], mean.Function, disperse);
        }
        return buffer;
    }
}
class EvolutionProcess
{
    public EvolutionProcess(NeuralNetwork network, Func<NeuralNetwork, int, bool> isNext, float disperse, List<NeuralNetwork> networks)
    {
        Network = network;
        Networks = networks;
        IsNext = isNext;
        Disperse = disperse;
        Count = Networks.Count;
        SaveCopy = Networks;
    }
    public void Next()
    {
        for (int i = 0; i < Networks.Count; i++)
        {
            Evolution.GetNeuralNetwork(Network, Disperse, Networks[i]);
        }
    }
    public void Selection(int stop, int length)
    {
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            var j = 0;
            while (Networks.Count > stop)
            {
                Networks.RemoveAll(n => IsNext(n, j));
                j++;
            }
            Network = Networks[random.Next(Count)];
            Networks = SaveCopy;
        }
    }
    public NeuralNetwork Network { get; private set; }
    public List<NeuralNetwork> Networks { get; private set; }
    private List<NeuralNetwork> SaveCopy { get; }
    public int Count { get; }
    public Func<NeuralNetwork, int, bool> IsNext { get; }
    public float Disperse { get; set; }
}