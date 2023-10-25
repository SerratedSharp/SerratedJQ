using System;

namespace SerratedSharp.JSInteropHelpers;

public static class ParamsHelpers
{
    // Shorthand for new object[]{,,,}
    public static T[] ToParams<T>(params T[] args)
    {
        return args;
    }

    public static object[] MergeParams(object[] args1, object[] args2)
    {
        object[] args = new object[args1.Length + args2.Length];
        Array.Copy(args1, args, args1.Length);
        Array.Copy(args2, 0, args, args1.Length, args2.Length);
        return args;
    }

    public static object[] Merge(object args1, object[] args2)
    {
        return MergeParams(new object[1] { args1 }, args2);
    }

    // Used when an overload takes a single param followed by a params [] array, and needs to prepend the single param to the param array
    public static A[] PrependToArray<A>(A arg, ref A[] args) //where A : class
    {
        if (args == null) {
            args = new A[0];// start with emptry array if was null
        }        
        Array.Resize(ref args, args.Length + 1);// make room for new item
        Array.Copy(args, 0, args, 1, args.Length - 1);// shift elements to the right to make room or new item
        args[0] = arg;// prepend item to beginning
        
        return args;
    }

}


