using SerratedSharp.JSInteropHelpers;

namespace SerratedSharp.SerratedJQ.Plain;


// TODO: A way to flag all types that are valid for JQuery "Content" parameter?
// Note we probably shouldn't inherit from IJSObjectWrapper, and instead let methods test
// content types for the interface on the fly
// Note: Using this as a return param should probably be discouraged because it lacks specificity.  It should typically only be an input param.
public interface IJQueryContentParameter:IJSObjectWrapper
{

}





