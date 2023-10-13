//using SerratedSharp.SerratedJQ;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Uno;
//using Wasm;

//namespace Tests.Wasm
//{

//    public class Events_Click : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'>&emsp;Click This</div>"));
//            child = tc.Find(".w");
//            child.OnClick += Child_OnClick;
//            child.OnClick += Child_OnClick2;
//            child.OnInput += Child_OnInput;
//            // TODO: Automatically trigger the event from JQuery, use set timeout to space out click requests?

//            tc.Append(JQueryBox.FromHtml($@"<br/>&emsp;&emsp;<span class='unsub3'>Click to unsubscribe ""Click This""</span>"));

//            tc.Find(".unsub3").OnClick += Child_UnSub3;
//        }

//        private static JQueryBox child;

//        private void Child_OnClick(JQueryBox sender, dynamic e)
//        {
//            string eventName = e.type;
//            Assert(eventName == "click");

//            tc.Append(JQueryBox.FromHtml($@"<br/>&emsp;<span style='font-size:8px'>Clicked, event: {e}</span>"));
//            tc.Append(JQueryBox.FromHtml($@"<br/>&emsp;&emsp;<span class='unsub'>Click to unsubscribe ""Click This""</span>"));

//            tc.Find(".unsub").OnClick += Child_UnSub;// wire up a second event handler that we can use to test unsubscribe

//        }

//        private void Child_OnClick2(JQueryBox sender, dynamic e)
//        {
//            string eventName = e.type;
//            Assert(eventName == "click");

//            tc.Append(JQueryBox.FromHtml($@"<br/>&emsp;&emsp;<span class='unsub2'>Click to unsubscribe ""Click This""</span>"));

//            tc.Find(".unsub2").OnClick += Child_UnSub2;
//        }

//        private void Child_OnInput(JQueryBox sender, dynamic e)
//        {
//            string eventName = e.type;
//            Assert(eventName == "input");
//        }

//        private void Child_UnSub(JQueryBox sender, dynamic e)
//        {
//            child.OnClick -= Child_OnClick;// if second element is clicked, then unsubscribe, verify after by clicking "Click This" should no longer work
//        }

//        private void Child_UnSub2(JQueryBox sender, dynamic e)
//        {
//            child.OnClick -= Child_OnClick2;// if second element is clicked, then unsubscribe, verify after by clicking "Click This" should no longer work
//        }

//        private void Child_UnSub3(JQueryBox sender, dynamic e)
//        {
//            child.OnInput -= Child_OnInput;// if second element is clicked, then unsubscribe, verify after by clicking "Click This" should no longer work
//        }
//    }

//    public class Events_MemoryLoadTest_SubscribeRemove : JQTest
//    {
//        public override void Run()
//        {
//            tc.AppendNew($"<div class='run'>Run Mem Test, Iterations: <span class='iterations'></span></div>");
//            var run = tc.Find($".run");

//            // We run this test 1000 items at a time because we have to free the thread to allow the remove observer to fire
//            // Running the test twice(once automatically and then once clicking) is usually sufficient to generate an out of memory exception if there is a problem with the reference management
//            run.OnClick += Run_OnClick; ;
//            Console.WriteLine("BeginTest");
//            for (int i = 0; i < 1000; i++)
//            {
//                SubscribeRemove(i);
//            }

//        }

//        private void Run_OnClick(JQueryBox sender, object e)
//        {
//            Console.WriteLine("BeginTest");
//            tc.Find(".run").AppendNew($"<span> + {tc.Find(".iterations").Text}</span>");

//            for (int i = 0; i < 1000; i++)
//            {
//                SubscribeRemove(i);
//            }
//        }

//        private void SubscribeRemove(int i)
//        {
//            tc.AppendNew($"<span class='w{i}'>Click Event Element</span>");

//            //Console.WriteLine($"MemLoad i:{i} i%50:{i % 50}");

//            var child = tc.Find($".w{i}");
//            var blah = new ListenerWithLargeMemoryFootprint();
//            child.OnClick += blah.Child_OnClick;

//            tc.Find($".w{i}").Remove();

//            if (i % 50 == 0)
//            {
//                tc.Find(".iterations").Text = i.ToString();
//                //JQueryBox.Select("#t1").AppendNew($"<div>{i}: MemLoad Objects {JQueryBox.eventObjectsByPointer.Count}</div>");
//                Console.WriteLine("MemLoad Objects: " + JQueryBox.eventObjectsByPointer.Count);
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//                //JQueryBox.Select("#t1").AppendNew($"<div>{i}: MemLoad After {JQueryBox.eventObjectsByPointer.Count}</div>");
//                Console.WriteLine("MemLoad After: " + JQueryBox.eventObjectsByPointer.Count);
//            }
//        }

//    }


//    public class Events_MemoryLoadTest_SubscribeUnsubscribe : JQTest
//    {
//        public override void Run()
//        {
//            tc.AppendNew($"<div class='run'>Run Mem Test, Iterations: <span class='iterations'></span></div>");
//            var run = tc.Find($".run");

//            // We run this test 1000 items at a time because we have to free the thread to allow the remove observer to fire
//            // Running the test twice(once automatically and then once clicking) is usually sufficient to generate an out of memory exception if there is a problem with the reference management
//            run.OnClick += Run_OnClick; ;
//            Console.WriteLine("BeginTest");
//            for (int i = 0; i < 1000; i++)
//            {
//                SubscribeUnsubscribe(i);
//            }

//        }

//        private void Run_OnClick(JQueryBox sender, object e)
//        {
//            Console.WriteLine("BeginTest");
//            tc.Find(".run").AppendNew($"<span> + {tc.Find(".iterations").Text}</span>");

//            for (int i = 0; i < 1000; i++)
//            {
//                SubscribeUnsubscribe(i);
//            }
//        }

//        const string minimalWidthStyle = "display:inline-block;width: 4px;max-width:4px;white-space:nowrap;overflow: hidden";
//        private void SubscribeUnsubscribe(int i)
//        {

//            tc.AppendNew($"<span class='w{i}' style='{minimalWidthStyle}'>Click Event Element</span>") ;

//            //Console.WriteLine($"MemLoad i:{i} i%50:{i % 50}");
//            var child = tc.Find($".w{i}");
//            var blah = new ListenerWithLargeMemoryFootprint();
//            child.OnClick += blah.Child_OnClick;

//            child.OnClick -= blah.Child_OnClick;
            
//            if (i % 100 == 0)
//            {
//                tc.Find(".iterations").Text = i.ToString();
//                //JQueryBox.Select("#t1").AppendNew($"<div>{i}: MemLoad Objects {JQueryBox.eventObjectsByPointer.Count}</div>");
//                Console.WriteLine("MemLoad Objects: " + JQueryBox.eventObjectsByPointer.Count);
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//                //JQueryBox.Select("#t1").AppendNew($"<div>{i}: MemLoad After {JQueryBox.eventObjectsByPointer.Count}</div>");
//                Console.WriteLine("MemLoad After: " + JQueryBox.eventObjectsByPointer.Count);
//            }
//        }


//        //// TODO: This test fails because subsequent tc.Find(".w") gets multiple items and all of the removed items have same pointer.  Not real clear why
//        //private void SomeTest(int i)
//        //{
//        //    tc.AppendNew("<span class='w'>Click This</span>");
//        //    //tc.Append("<div class='w'>Click This</div>"));
//        //    var child = tc.Find(".w");
//        //    var blah = new SomeListener();
//        //    child.OnClick += blah.Child_OnClick;
//        //    //tc.Remove();
//        //    //Console.WriteLine("MemLoad BeforeRemove" + JQueryBox.eventObjectsByPointer.Count);
//        //    //tc

//        //    //tc.Remove(".w:nth-of-type(1)");
//        //    //Console.WriteLine("MemLoad AfterRemove" + JQueryBox.eventObjectsByPointer.Count);
//        //    //Console.WriteLine(JQueryBox.eventObjectsByPointer.Count)
//        //    //Console.WriteLine("Size: " + sizeof(SomeListener));

//        //    //if(i % 1)
//        //    //Console.WriteLine("Total Mem: " + GC.GetTotalMemory(false));

//        //    if (i % 100 == 0)
//        //    {

//        //        Console.WriteLine("MemLoad Objects: " + JQueryBox.eventObjectsByPointer.Count);
//        //        GC.Collect();
//        //        Console.WriteLine("MemLoad After: " + JQueryBox.eventObjectsByPointer.Count);
//        //    }
//        //}

//        //private void Child_OnClick(JQueryBox sender, dynamic e)
//        //{
//        //    string eventName = e.type;
//        //    sender.Append(JQueryBox.FromHtml($"<span> Clicked{e}</span>"));
//        //    Assert(eventName == "click");
//        //}

//    }

//    // This is the listener that is prevented from GC'ing by pinning internally
//    public class ListenerWithLargeMemoryFootprint
//    {
//        public Int32[] ints = new Int32[100000];
//        public void Child_OnClick(JQueryBox sender, dynamic e)
//        {
//            string eventName = e.type;
//            sender.Append(JQueryBox.FromHtml($"<span> Clicked{e}</span>"));
//            //Assert(eventName == "click");
//        }
//    }
//}
