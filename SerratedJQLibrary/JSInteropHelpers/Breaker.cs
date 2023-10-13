namespace SerratedSharp.JSInteropHelpers.Internal;

/// <summary>
/// Disambiguate method parameters when multiple overloads take a params array and optional parameters with caller info.  See https://stackoverflow.com/a/26784846/84206
/// </summary>
public struct Breaker { } // Prevents incorrect overload being used. 

