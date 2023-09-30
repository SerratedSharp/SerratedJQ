using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJQ
{
    public interface IJSObjectWrapper
    {
        JSObject JSObject { get; }
    }

    public interface IJSObjectWrapper<W> : IJSObjectWrapper where W : IJSObjectWrapper<W>
    {
        // Generic factory method for taking JSObject and wrapping it
        static abstract W WrapInstance(JSObject jsObject);
    }



}

