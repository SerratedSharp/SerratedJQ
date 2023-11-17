using SerratedSharp.JSInteropHelpers;
using SerratedSharp.JSInteropHelpers.Internal;
using System.Collections.Generic;

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
        get => jsWrapper.CallJSOfSameName<TValue1>(key, default(Breaker), funcName);
        set => jsWrapper.CallJSOfSameName<object>(key, value, default(Breaker), funcName);
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
        get => jsWrapper.CallJSOfSameName<TValue1>(key, default(Breaker), funcName);
        set => jsWrapper.CallJSOfSameName<object>(key, value, default(Breaker), funcName);
    }

    public TValue2 this[TKey2 key]
    {
        get => jsWrapper.CallJSOfSameName<TValue2>(key, default(Breaker), funcName);
        set => jsWrapper.CallJSOfSameName<object>(key, value, default(Breaker), funcName);
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
        get => jsWrapper.CallJSOfSameName<TValue1>(key, default(Breaker), funcName);
    }
}
