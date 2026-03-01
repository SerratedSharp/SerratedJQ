using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedJQ;

public class JQIndexer<TWrapper, TKey1, TValue1> where TWrapper : IJSObjectWrapper<TWrapper>
{
    private readonly TWrapper jsWrapper;
    private readonly string funcName;

    public JQIndexer(TWrapper jsWrapper, string funcName)
    {
        this.jsWrapper = jsWrapper;
        this.funcName = funcName;
    }

    public TValue1 this[TKey1 key]
    {
        get => jsWrapper.CallJS<TValue1>(funcName: funcName, key);
        set => jsWrapper.CallJS<object>(funcName: funcName, key, value);
    }
}

// An indexer supporting two different key types
public class JQIndexer<TWrapper, TKey1, TValue1, TKey2, TValue2> where TWrapper : IJSObjectWrapper
{
    private readonly TWrapper jsWrapper;
    private readonly string funcName;

    public JQIndexer(TWrapper jsWrapper, string funcName)
    {
        this.jsWrapper = jsWrapper;
        this.funcName = funcName;
    }

    public TValue1 this[TKey1 key]
    {
        get => jsWrapper.CallJS<TValue1>(funcName: funcName, key);
        set => jsWrapper.CallJS<object>(funcName: funcName, key, value);
    }

    public TValue2 this[TKey2 key]
    {
        get => jsWrapper.CallJS<TValue2>(funcName: funcName, key);
        set => jsWrapper.CallJS<object>(funcName: funcName, key, value);
    }
}

public class ReadOnlyJQIndexer<TWrapper, TKey1, TValue1> where TWrapper : IJSObjectWrapper<TWrapper>
{
    private readonly TWrapper jsWrapper;
    private readonly string funcName;

    public ReadOnlyJQIndexer(TWrapper jsWrapper, string funcName)
    {
        this.jsWrapper = jsWrapper;
        this.funcName = funcName;
    }

    public TValue1 this[TKey1 key]
    {
        get => jsWrapper.CallJS<TValue1>(funcName: funcName, key);
    }
}
