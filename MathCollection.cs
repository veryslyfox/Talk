using System.Collections;

class MathCollection : IEnumerable<float>
{
    public MathCollection(float[] values)
    {
        Values = values;
    }
    public MathCollection(Func<int, float> func, int length)//?
    {
        Values = new float[length];
        for (int i = 0; i < length - 1; i++)
        {
            Values[i] = func(i);
        }
    }
    public MathCollection Difference()//?
    {
        if (Values.Length == 1)
        {
            var a = this;
            a.Values[0] = Math.Abs(a.Values[0]);
            return a;
        }
        if (Values.Length == 0)
        {
            return new MathCollection(new float[]{0});
        }
        return new(n => Math.Abs(Values[n + 1] - Values[n]), Values.Length - 1);
    }

    public IEnumerator<float> GetEnumerator()
    {
        return ((IEnumerable<float>)Values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    static public implicit operator float[](MathCollection value)
    {
        return value.Values;
    }
    static public implicit operator MathCollection(float[] value)
    {
        return new(value);
    }
    public float[] Values { get; }
}
static class MathEnumerable
{

}