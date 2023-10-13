//using SerratedSharp.SerratedJQ;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Wasm;

//namespace Tests.Wasm
//{


//    public class Traversal_TreeTraversal_Children1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div></div><div></div><span></span>"));
//            var t = tc.Children("div");
//            //Console.WriteLine(t.Html);
//            Assert(t.Length == 2);
//        }
//    }


//    public class Traversal_TreeTraversal_Children2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div></div><div></div><span></span>"));
//            var t = tc.Children();
//            //Console.WriteLine(t.Html);
//            Assert(t.Length == 3);
//        }
//    }

//    public class Traversal_TreeTraversal_Closest : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div></div>"));
//            var t = tc.Closest("div");
//            //Console.WriteLine(t.Html);
//            Assert(t.Length == 1);
//            Assert(t.HasClass("tc"));
//        }
//    }

//    public class Traversal_TreeTraversal_Find : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div><b></b></div><b></b>"));
//            var t = tc.Find("b");
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_Next1 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='next'></div><div class='next2'></div>"));
//            var t = child.Next();
//            Assert(t.HasClass("next"));
//            Assert(t.Length == 1);
//        }
//    }

//    public class Traversal_TreeTraversal_Next2 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='next'></div><div class='next2'></div>"));
//            var t = child.Next(".next");
//            Assert(t.HasClass("next"));
//            Assert(t.Length == 1);
//        }
//    }

//    public class Traversal_TreeTraversal_NextAll1 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='next'></div><div class='next2'></div><div class='next'></div>"));
//            var t = child.NextAll();
//            Assert(t.Length == 3);
//        }
//    }

//    public class Traversal_TreeTraversal_NextAll2 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='next'></div><div class='next2'></div><div class='next'>"));
//            var t = child.NextAll(".next");
//            Assert(t.HasClass("next"));
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_NextUntil1 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='y x'></div><div class='x'></div><div class='z'></div>"));
//            var t = child.NextUntil();
//            Assert(t.Length == 3);
//        }
//    }

//    public class Traversal_TreeTraversal_NextUntil2 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='y x'></div><div class='x'></div><div class='z'></div>"));
//            var t = child.NextUntil(".z");
//            Assert(t.Length == 2);
//        }
//    }


//    public class Traversal_TreeTraversal_NextUntil3 : JQTest
//    {
//        public override void Run()
//        {
//            var child = JQueryBox.FromHtml("<div></div>");
//            tc.Append(child);
//            tc.Append("<div class='y x'></div><div class='x'></div><div class='z'></div>"));
//            var t = child.NextUntil(".z", ".y");
//            Assert(t.HasClass("y"));
//            Assert(t.Length == 1);
//        }
//    }

//    public class Traversal_TreeTraversal_ClosestPositioned : JQTest
//    {
//        public override void Run()
//        {
//            var parents = JQueryBox.FromHtml("<div style='position:relative;' class='win'><div class='y x'></div><div class='x'><div class='z'></div></div></div>");
//            tc.Append(parents);
//            var child = tc.Find(".z");
//            var t = child.ClosestPositioned();
//            Assert(t.HasClass("win"));
//            Assert(t.Length == 1);
//        }
//    }


//    public class Traversal_TreeTraversal_Parents1 : JQTest
//    {
//        public override void Run()
//        {
//            var parents = JQueryBox.FromHtml("<div class='win'><div class='y x'></div><div class='win'><div class='z'></div></div></div>");
//            tc.Append(parents);
//            var child = tc.Find(".z");
//            var t = child.Parents(".win");
//            Assert(t.HasClass("win"));
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_Parents2 : JQTest
//    {
//        public override void Run()
//        {
//            var parents = JQueryBox.FromHtml("<div class='win'><div class='y x'></div><div class='x'><div class='z'></div></div></div>");
//            tc.Append(parents);
//            var child = tc.Find(".z");
//            var t = child.Parents();
//            Assert(t.Length > 2);
//        }
//    }

//    public class Traversal_TreeTraversal_ParentsUntil1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='y x'><div class='x'><div class='z'></div></div></div>"));
//            var child = tc.Find(".z");
//            var t = child.ParentsUntil();
//            Assert(t.Length > 2);
//        }
//    }

//    public class Traversal_TreeTraversal_ParentsUntil2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='y x'><div class='x'><div class='z'></div></div></div>"));
//            var child = tc.Find(".z");
//            var t = child.ParentsUntil(".y");
//            Assert(t.Length == 1);
//        }
//    }

//    public class Traversal_TreeTraversal_ParentsUntil3 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='y x'><div class='x'><div class='z'></div></div></div>"));
//            var child = tc.Find(".z");
//            var t = child.ParentsUntil(".y", ".x");
//            Assert(t.Length == 1);
//        }
//    }


//    public class Traversal_TreeTraversal_PrevAll1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.PrevAll(".x");
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_PrevAll2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='x'></div><div class='y'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.PrevAll();
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_PrevUntil1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.PrevUntil(".w");
//            Assert(t.Length == 3);
//        }
//    }

//    public class Traversal_TreeTraversal_PrevUntil2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.PrevUntil();
//            Assert(t.Length == 4);
//        }
//    }


//    public class Traversal_TreeTraversal_PrevUntil3 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.PrevUntil(".w", ".x");
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_Siblings1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".y");
//            var t = child.Siblings(".x");
//            Assert(t.Length == 2);
//        }
//    }

//    public class Traversal_TreeTraversal_Siblings2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='x'></div><div class='z'></div>"));
//            var child = tc.Find(".y");
//            var t = child.Siblings();
//            Assert(t.Length == 4);
//        }
//    }

//    public class Traversal_TreeTraversal_Parent : JQTest
//    {
//        public override void Run()
//        {
//            var parents = JQueryBox.FromHtml("<div class='win'><div class='y x'></div><div class='x'><div class='z'></div></div></div>");
//            tc.Append(parents);
//            var child = tc.Find(".z");
//            var t = child.Parent(".x");
//            Assert(t.HasClass("x"));
//            Assert(t.Length == 1);
//        }
//    }

//    public class Traversal_TreeTraversal_Prev1 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='x'></div><div class='y'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.Prev();
//            Assert(t.Length == 1);
//            Assert(t.HasClass("y"));
//        }
//    }

//    public class Traversal_TreeTraversal_Prev2 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='x'></div><div class='y'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.Prev(".y");
//            Assert(t.Length == 1);
//            Assert(t.HasClass("y"));
//        }
//    }

//    public class Traversal_TreeTraversal_Prev3 : JQTest
//    {
//        public override void Run()
//        {
//            tc.Append("<div class='x'></div><div class='y'></div><div class='z'></div>"));
//            var child = tc.Find(".z");
//            var t = child.Prev(".x");
//            Assert(t.Length == 0);
//        }
//    }




//}

