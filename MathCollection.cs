using System.Collections;

class MathCollection : IEnumerable<int>
{
    public MathCollection(int[] values)
    {
        Values = values;
    }
    public MathCollection(Func<int, int> func, int length)//?
    {
        Values = new int[length];
        for (int i = 0; i < length - 1; i++)
        {
            Values[i] = func(i);
        }
    }
    public MathCollection Difference()//?
    {
        if (Values.Length == 1)
        {
            return this;
        }
        if (Values.Length == 0)
        {
            return new MathCollection(new int[]{0});
        }
        return new(n => Math.Abs(Values[n + 1] - Values[n]), Values.Length - 1);
    }

    public IEnumerator<int> GetEnumerator()
    {
        return ((IEnumerable<int>)Values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    static public implicit operator int[](MathCollection value)
    {
        return value.Values;
    }
    static public implicit operator MathCollection(int[] value)
    {
        return new(value);
    }
    public int[] Values { get; }
}
static class MathEnumerable
{

}