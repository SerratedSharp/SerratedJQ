//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Uno.Foundation;

//namespace SerratedSharp.SerratedJQ
//{
//    //[System.AttributeUsage(System.AttributeTargets.Method)]
//    //public class ExportToJavascriptAttribute: Attribute
//    //{
//    //    private readonly string jsNamespace;
//    //    internal string JsNamespace => jsNamespace;

//    //    public ExportToJavascriptAttribute(string jsNamespace = null)
//    //    {
//    //        this.jsNamespace = jsNamespace;
//    //    }

        
//    //}


//    /// <summary>
//    /// Creates a javascript method that proxies calls to a .NET method. Used to expose .NET method for calling from javascript.
//    /// </summary>
//    public static class CallbacksHelper
//    {
//        private const string InternalSJQ = "InternalSJQ";
//        private const string PublicNamespace = "Serrated";
//        private static bool keep;
//        static CallbacksHelper()
//        {
//            WebAssemblyRuntime.InvokeJS(@$"     
//                    window.{InternalSJQ} = window.{InternalSJQ} || {{}};

//                    window.{InternalSJQ}.Listener = Module.mono_bind_static_method('[SerratedSharp.SerratedJQ] SerratedSharp.SerratedJQ.CallbacksHelper:Listener');
                         
//                ");

//        }

//        static Dictionary<string, CallbackFunc> Callbacks { get; set; } = new Dictionary<string, CallbackFunc>();

//        public delegate void CallbackFunc();//params string[] args);

//        /// <summary>
//        /// Makes a C# delegate available to call from javascript.  Available in javascript as <c>Serrated.Callbacks.SomeMethod()</c> where "SomeMethod" is the value of <paramref name="jsMethodName"/>
//        /// <example><code>Export("SomeMethod", () => Console.WriteLine("C# called from Javascript"));</code></example>
//        /// </summary>
//        /// <param name="jsMethodName">The javascript method name to expose on Serrated.Callbacks</param>
//        /// <param name="callbackPredicate">C# delegate to execute when the javascript method is called.</param>
//        /// <param name="overwriteDuplicates">By default a duplicate registration with the same <paramref name="jsMethodName"/> generates an exception.  If true, then duplicate method names are overwritten instead.</param>
//        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CallbacksHelper))]
//        public static void Export(string jsMethodName, CallbackFunc callbackPredicate, bool overwriteDuplicates =false)
//        {
            
//            if (Callbacks.ContainsKey(jsMethodName))
//            {
//                if(!overwriteDuplicates)
//                    throw new Exception($"Method name {jsMethodName} already registered.");
//            }
//            else
//            {

//                //WebAssemblyRuntime.InvokeJS(@$"       
//                //    window.{jsMethodName} = function(){{
//                //         SerratedJQInternal('{jsMethodName}');
//                //    }}
//                //");
//                StringBuilder exportJS = new StringBuilder();
//                WrapSingle(exportJS,
//                    new JsFunctionDeclaration
//                    {
//                        ObjectName = "Callbacks",
//                        FunctionName = jsMethodName,
//                        FunctionBody = $@"function(){{
//                            {InternalSJQ}.Listener('{jsMethodName}');
//                        }}"
//                    }
//                    , PublicNamespace
//                    );
//                //Console.WriteLine(exportJS.ToString() );
//                WebAssemblyRuntime.InvokeJS(exportJS.ToString());
//            }

//            Callbacks[jsMethodName] = callbackPredicate;

//        }


//        /// <summary>
//        /// Do not call.
//        /// </summary>        
//        public static void Listener(string jsMethodName)
//        {
//            Callbacks[jsMethodName]();
//        }


//        /// <summary>
//        /// Scans the class for static methods marked with ExportToJavascript attribute.
//        /// Methods must be public, static, and have only string parameters.
//        /// </summary>
//        /// <param name="classToScan"><c>typeof(YourClass)</c></param>
//        /// <returns>A semicolon delimited list of fully qualified method names exported.</returns>
//        //public static string ExportTaggedMethods(Type classToScan)
//        //{

//        //    var methodsToExport = classToScan.GetMethods()
//        //    .Where(m => m.GetCustomAttributes(typeof(ExportToJavascriptAttribute), true).Length > 0)
//        //    .ToList();

//        //    foreach(var method in methodsToExport)
//        //    {
//        //        // TODO: Use namespace from attribute
//        //         string[] names = method.DeclaringType.FullName.Split(".");
//        //        StringBuilder exportJS = new StringBuilder();

//        //        string name = names.First();

//        //        // start
//        //        exportJS.Append($"var {name} = {name} || {{}};");
//        //        exportJS.Append($"(function ({name}) {{");

//        //        WrapRecurse(1, names, exportJS);

//        //        // end
//        //        exportJS.Append($"}})({name} || ({name} = {{}}));");


//        //        exportJS.ToString();

//        //    }



//        //    WebAssemblyRuntime.InvokeJS(@"
//        //        isReady = false;
//        //        var script = document.createElement('script');
//        //        script.onload = function () {                    
//        //            var beginTests = Module.mono_bind_static_method('[Tests.Wasm] Wasm.Program:Begin');
//        //            beginTests();
//        //        };
//        //        script.src = 'https://code.jquery.com/jquery-3.6.0.js';
//        //        document.head.appendChild(script);
//        //    ");
//        //    return "TODO:";

//        //}

//        //private static StringBuilder WrapRecurse(int currentIndex, string [] names, StringBuilder innerJS, StringBuilder outerJS = null)
//        //{
//        //    string name = names[currentIndex];
//        //    string parentName = names[currentIndex-1];

//        //    // open
//        //    exportJS.Append($"var {name};");
//        //    exportJS.Append($"(function ({name}) {{");

//        //    if(currentIndex < names.Length)
//        //        WrapRecurse(currentIndex+1, names, exportJS);

//        //    // close            
//        //    exportJS.Append($"}})({name} = {parentName}.{name} || ({parentName}.{name} = {{}}));");

//        //    return exportJS;
//        //}

//        private static StringBuilder WrapSingle(StringBuilder exportJS, JsFunctionDeclaration jsFunction, string nameSpace, string parentName="window")
//        {

//            // open
//            exportJS.Append($"var {nameSpace} = {parentName}.{nameSpace} || {{}};");
//            exportJS.Append($"(function ({nameSpace}) {{");

//            WrapObjectFunc(exportJS, jsFunction, nameSpace);

//            // close            
//            exportJS.Append($"}})({nameSpace} = {parentName}.{nameSpace} || ({parentName}.{nameSpace} = {{}}));");

//            return exportJS;
//        }

//        private class JsFunctionDeclaration
//        {
//            public string ObjectName { get; set; }
//            public string FunctionName { get; set; }
//            public string FunctionBody { get; set; }
//        }

//        /// <summary>Adds a public static function to JS object</summary>
//        /// <param name="functionBody">function(string blah){ console.log(blah); }</param>
//        private static StringBuilder WrapObjectFunc(StringBuilder exportJS, JsFunctionDeclaration jsFunction, string parentName = "window")
//        {
            
//            exportJS.Append($"var {jsFunction.ObjectName} = {parentName}.{jsFunction.ObjectName} || {{}};");
//            exportJS.Append($"{jsFunction.ObjectName}.{jsFunction.FunctionName} = {jsFunction.FunctionBody};");
//            exportJS.Append($"{parentName}.{jsFunction.ObjectName} = {jsFunction.ObjectName};");
            
//            return exportJS;

//        }

//    }




//}
