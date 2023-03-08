//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using Uno.Extensions;
//using Uno.Foundation;
//using Uno.Foundation.Interop;
//using static System.Console;
//using System.Linq;

//namespace WasmGenerator
//{

//    internal class JSObject : IJSObject
//    {

//        private JSObjectBox ()
//        {
//            Handle = JSObjectHandle.Create(this);
//        }

//        public JSObjectHandle Handle { get; }


//        // static because not called on an existing JQuery object instance
//        public static JQueryBox Box(string selector)
//        {
//            var box = new JSObject();

//            // Call $(selector) and add the return to the new JS JQueryBox.jqObj
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{box}.jsObj = $('{selector}');"
//                );

//            return box;
//        }

//        // non-static instance method called on this current instance
//        public void Hide() => WebAssemblyRuntime.InvokeJSWithInterop($"{this}.jqObj.hide();");


//        public JQueryBox SetStyle(string propertyName, string value)
//        {
//            var newBox = new JQueryBox();
//            // Call Find on the current instance, and add the new instance returned from find()
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.css('{propertyName}','{value}');"
//            );

//            // InvokeJSWithInterop has special behavior when used with string interpolation. 
//            // It is able to inspect the interpolated string via FormatableString, which allows it to detect
//            // the references to {newBox} and {this} as IJSObject instances, and replace them with .getInstance calls against the
//            // client side activeInstances array.  The resulting javascript is an immediately invoked function expression. 
//            // This pattern is used simply to execute the snippet and ensure the local variables don't pollute global.
//            // Since {propertyName} and {value} are string types, it performs no special replacements and standard string interpolation behavior applies.

//            //(function() {
//            //    var __parameter_0 = WasmGenerator.JQueryBox.getInstance("162", "4");
//            //    var __parameter_1 = WasmGenerator.JQueryBox.getInstance("130", "3");
//            //    __parameter_0.jqObj = __parameter_1.jqObj.css('color', 'red');
//            //    return "ok";
//            //})();

//            return newBox;
//        }

//        /// <summary>
//        /// Returns the parent container.
//        /// </summary>        
//        public JQueryBox Append(string html)
//        {
//            // Escape single quotes and remove control characters that break append
//            html = html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
//            //Console.WriteLine(html);
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.append('{html}');"
//            );
//            return newBox;
//        }

//        /// <summary>
//        /// Returns reference to the appended item(s).
//        /// </summary>        
//        public JQueryBox AppendTo(string html)
//        {
//            // Escape single quotes and remove control characters that break append
//            html = html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = $('{html}').appendTo({this}.jqObj);"
//            );
//            return newBox;
//        }


//        public JQueryBox AddClass(string className)
//        {
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.addClass('{className}');"
//            );
//            return newBox;
//        }

//        public JQueryBox RemoveClass(string className)
//        {
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.removeClass('{className}');"
//            );
//            return newBox;
//        }


//        public long Length()
//        {
//            return Convert.ToInt64( WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"return {this}.jqObj.length;"
//            ));
//        }


//        public JQueryBox On(string events, IJSObject eventListener, string listenerFunctionName)
//        {
//            var newBox = new JQueryBox();
//            // Call Find on the current instance, and add the new instance returned from find()
//            WebAssemblyRuntime.InvokeJSWithInterop($@"

//                    var handler = function(e){{
//                        var eEncoded = btoa(JSON.stringify(e));                        
//                        {eventListener}.{listenerFunctionName}(eEncoded);
//                    }}.bind({eventListener});

//                    {newBox}.jqObj = {this}.jqObj.on('{events}', handler);
//                ");
//            return newBox;
//        }


//        public string ValueGet()
//        {
//            return WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"return {this}.jqObj.val();"
//            );
//            // TODO: .val() can return string, number, or array: https://api.jquery.com/val/#val1
//        }

//        /// <summary>
//        /// Returns the parent container.
//        /// </summary>        
//        public JQueryBox Prepend(string html)
//        {
//            // Escape single quotes and remove control characters that break append
//            html = html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
//            //Console.WriteLine(html);
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.prepend('{html}');"
//            );
//            return newBox;
//        }

//        /// <summary>
//        /// Returns reference to the appended item(s).
//        /// </summary>        
//        public JQueryBox PrependTo(string html)
//        {
//            // Escape single quotes and remove control characters that break append
//            html = html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = $('{html}').prependTo({this}.jqObj);"
//            );
//            return newBox;
//        }


//        public JQueryBox Text(string text)
//        {
//            // Escape single quotes and remove control characters that break append
//            text = text.Replace("'", "\\'");
//            var newBox = new JQueryBox();
//            WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"{newBox}.jqObj = {this}.jqObj.text('{text}');"
//            );
//            return newBox;
//        }

//        // Due to limitations we can't support overloaded methods, so different signatures need different names.
//        public string TextGet()
//        {
//            string text = WebAssemblyRuntime.InvokeJSWithInterop(
//                $@"return {this}.jqObj.text();");
//            return text;
//        }

//        // This method demonstrates a brute force approach to using the JS handles. It might be useful in edge cases.
//        public JQueryBox Find(string selector)
//        {
//            long currentJsHandle = _jsHandle(Handle);

//            var newBox = new JQueryBox();
//            var newJsHandle = _jsHandle(newBox.Handle);

//            // Call Find on the current instance, and add the new instance returned from find()
//            WebAssemblyRuntime.InvokeJS($@"
//                (function() {{
//                    var current = WasmGenerator.{GetType().Name}.getInstance(0,{currentJsHandle});
//                    var newObj = WasmGenerator.{GetType().Name}.getInstance(0,{newJsHandle});
//                    newObj.jqObj = current.jqObj.find('{selector}');
//                    return 'OK';
//                }})();"
//            );

//            return newBox;
//        }

//        // TODO: Some JQuery methods actually return the same instance.  We should consider testing the JS object equality
//        // and returning this instead of newBox in those cases and explicitely Dispose of newBox.


//        public string GenGetInstance()
//        {
//            var jsHandle = _jsHandle(Handle);
//            return $"WasmGenerator.{ GetType().Name}.getInstance(0,{jsHandle})";
//        }

//        // Get jsHandle ID via reflection
//        static long _jsHandle(JSObjectHandle jsHandle)
//        {
//            return GetPrivate<long>(jsHandle);
//        }

//        static IntPtr _managedHandle(JSObjectHandle jsHandle)
//        {
//            return GetPrivate<IntPtr>(jsHandle);
//        }

//        // Reflection to access private fields needed for getting JS handle ID when making instance interopt calls
//        static T GetPrivate<T>(object obj, [CallerMemberName] string callerMemberName = null)
//        {
//            var field = obj.GetType().GetField(callerMemberName, BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//            var value = (T)field.GetValue(obj);
//            return value;
//        }

//        // Debugging: C# methods that are exposed to JS must meet this criteria
//        public static void EchoValidMethods()
//        {
//            var methods = typeof(JQueryBox)
//                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
//                    .Where(method => !method.GetParameters().Any(p => p.ParameterType != typeof(string))) // we support only string parameters for now
//                    .ToList();

//            Console.WriteLine(string.Join(", ", methods.Select(m => m.Name)));
//        }

//    }


//}
