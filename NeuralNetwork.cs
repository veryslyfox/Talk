using static System.MathF;
using static Error;
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
    public float[] Use(float[] data)
    {
        this[0].Neurons = IsNull(data.Select(value => new Neuron(value)).ToList(), "null input");
        for (NeuronLayer i = this[0]; i.Next != null; i = i.Next)
        {
            for (int j = 0; j < i.Neurons.Count; j++)
            {
                i.Neurons[j].WriteValue();
            }
            if (i.Next == null)
            {
                return IsNull(i.Neurons.Select(n => n.Value), "null output", new float[] { }).ToArray();
            }
        }
        return new float[] { 0 };
    }
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
        set => NeuronLayers![index] = value;
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
#pragma warning disable //
    public Neuron(float value)
#pragma warning restore
    {
        Value = value;
    }
    public void WriteValue()
    {

        if (Layer.Neurons == null)
        {
            Error.Add("neurons not init");
            return;
        }

        var collection = Weights.Zip(IsNull(Layer.Neurons, "neurons not init"), (float i, Neuron neuron) => i * neuron.Value);
        if (collection == null || collection.Count() == 0)
        {
            Error.Add("neuron not get data");
            Value = 0;
            return;
        }
        Value = Activate(collection.Sum(), ActivateFunction);
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

    public List<Neuron> Neurons { get; set; }
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
    public void Selection(int length)
    {
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            Networks.RemoveAll(n => !IsNext(n, 1));
            if (Networks.Count == 0)
            {
                Error.Add("Empty selection space");
                Networks = SaveCopy;
                return;
            }
            Network = Networks[random.Next(Count)];
        }
        Networks = SaveCopy;
    }
    public NeuralNetwork Network { get; private set; }
    public List<NeuralNetwork> Networks { get; private set; }
    private List<NeuralNetwork> SaveCopy { get; }
    public int Count { get; }
    public Func<NeuralNetwork, int, bool> IsNext { get; }
    public float Disperse { get; set; }
}
