using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class SpanVsListBenchmark
{
    private int[] _data = [];
    private List<int> _listData = [];

    [Params(1000, 10000, 100000)] // Test with different dataset sizes
    public int DataSize;

    [GlobalSetup]
    public void Setup()
    {
        _data = new int[DataSize];
        _listData = new List<int>(DataSize);
        for (int i = 0; i < DataSize; i++)
        {
            _data[i] = i;
            _listData.Add(i);
        }
    }

    // 1. Indexing
    [Benchmark]
    public int ListIndexing()
    {
        int sum = 0;
        for (int i = 0; i < _listData.Count; i++)
        {
            sum += _listData[i];
        }
        return sum;
    }

    [Benchmark]
    public int SpanIndexing()
    {
        Span<int> spanData = _data.AsSpan();
        int sum = 0;
        for (int i = 0; i < spanData.Length; i++)
        {
            sum += spanData[i];
        }
        return sum;
    }

    [Benchmark]
    public int ReadOnlySpanIndexing()
    {
        ReadOnlySpan<int> spanData = _data.AsSpan();
        int sum = 0;
        for (int i = 0; i < spanData.Length; i++)
        {
            sum += spanData[i];
        }
        return sum;
    }

    // 2. Iteration
    [Benchmark]
    public int ListIteration()
    {
        int sum = 0;
        foreach (var item in _listData)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int SpanIteration()
    {
        Span<int> spanData = _data.AsSpan();
        int sum = 0;
        foreach (var item in spanData)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int ReadOnlySpanIteration()
    {
        ReadOnlySpan<int> spanData = _data.AsSpan();
        int sum = 0;
        foreach (var item in spanData)
        {
            sum += item;
        }
        return sum;
    }

    // 3. Slicing
    [Benchmark]
    public int ListSlicing()
    {
        var slice = _listData.GetRange(0, _listData.Count / 2);
        int sum = 0;
        foreach (var item in slice)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int SpanSlicing()
    {
        Span<int> spanData = _data.AsSpan();
        var slice = spanData.Slice(0, spanData.Length / 2);
        int sum = 0;
        foreach (var item in slice)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int ReadOnlySpanSlicing()
    {
        ReadOnlySpan<int> spanData = _data.AsSpan();
        var slice = spanData.Slice(0, spanData.Length / 2);
        int sum = 0;
        foreach (var item in slice)
        {
            sum += item;
        }
        return sum;
    }

    // 4. Searching
    [Benchmark]
    public bool ListSearching()
    {
        return _listData.Contains(DataSize / 2);
    }

    [Benchmark]
    public bool SpanSearching()
    {
        Span<int> spanData = _data.AsSpan();
        for (int i = 0; i < spanData.Length; i++)
        {
            if (spanData[i] == DataSize / 2)
                return true;
        }
        return false;
    }

    [Benchmark]
    public bool ReadOnlySpanSearching()
    {
        ReadOnlySpan<int> spanData = _data.AsSpan();
        for (int i = 0; i < spanData.Length; i++)
        {
            if (spanData[i] == DataSize / 2)
                return true;
        }
        return false;
    }

    // 5. Memory Copying
    [Benchmark]
    public void ListMemoryCopy()
    {
        var copy = new List<int>(_listData);
    }

    [Benchmark]
    public void SpanMemoryCopy()
    {
        Span<int> spanData = _data.AsSpan();
        Span<int> destination = stackalloc int[spanData.Length];
        spanData.CopyTo(destination);
    }

    [Benchmark]
    public void ReadOnlySpanMemoryCopy()
    {
        ReadOnlySpan<int> spanData = _data.AsSpan();
        Span<int> destination = stackalloc int[spanData.Length];
        spanData.CopyTo(destination);
    }

    // 6. Modifications
    [Benchmark]
    public void ListModification()
    {
        for (int i = 0; i < _listData.Count; i++)
        {
            _listData[i] = _listData[i] * 2;
        }
    }

    [Benchmark]
    public void SpanModification()
    {
        Span<int> spanData = _data.AsSpan();
        for (int i = 0; i < spanData.Length; i++)
        {
            spanData[i] = spanData[i] * 2;
        }
    }
}

class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<SpanVsListBenchmark>();
    }
}