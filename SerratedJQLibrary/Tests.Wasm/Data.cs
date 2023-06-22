using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasm;

namespace Tests.Wasm
{

    public class Data_DataBag1 : JQTest
    {
        public Data_DataBag1()
        {
            IsModelTest = false;
        }

        public class TestModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }

        }


        public override void Run()
        {
            var uiModel = JQueryBox.FromHtml("<div class='w'></div>");
            tc.Append(uiModel);

            var child = tc.Find(".w");

            child.DataBag.One = 1;
            child.DataBag.Two = 2;
            Console.WriteLine("Databag" + child.DataBag.Two);
            Console.WriteLine("Databag" + child.DataBag.One);
            Assert(child.DataBag.One + child.DataBag.Two == 3);



        }

    }



    public class Data_DataProperty1 : JQTest
    {
        public Data_DataProperty1()
        {
            IsModelTest = false;
        }

        public class TestModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }

        }


        public override void Run()
        {
            var uiModel = JQueryBox.FromHtml("<div class='w'></div>");
            tc.Append(uiModel);

            uiModel.DataAdd("someModel", new TestModel { UserId = 3, Name = "Blah" });

            var child = tc.Find(".w");

            var model2 = child.DataGet<TestModel>("someModel");

            Assert(model2.Name == "Blah");
            Assert(model2.UserId == 3);

        }

    }



    public class Data_DataProperty2 : JQTest
    {
        public Data_DataProperty2()
        {
            IsModelTest = false;
        }

        public class TestModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }

        }


        public override void Run()
        {
            var uiModel = JQueryBox.FromHtml("<div class='w'></div>");
            tc.Append(uiModel);

            uiModel.DataAdd("someModel", "simplestring");

            var child = tc.Find(".w");

            var model2 = child.DataGet<string>("someModel");

            Assert(model2 == "simplestring");

        }

    }


    public class Data_DataProperty3 : JQTest
    {
        public Data_DataProperty3()
        {
            IsModelTest = false;
        }

        public class TestModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }

        }


        public override void Run()
        {
            var uiModel = JQueryBox.FromHtml("<div class='w'></div>");
            tc.Append(uiModel);

            uiModel.DataAdd("someModel", 3);

            var child = tc.Find(".w");

            var model2 = child.DataGet<int>("someModel");

            Assert(model2 == 3);

        }

    }



    public class Data_DataExperimental1 : JQTest
    {
        public Data_DataExperimental1()
        {
            IsModelTest = false;
        }

        public class TestModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }

        }


        public override void Run()
        {
            var model = new TestModel { UserId = 3, Name = "Blah" };

            var uiModel = JQueryBox.FromHtml("<div class='w'></div>");
            tc.Append(uiModel);
            uiModel.ManagedObjectAttach("KeyA", model);

            var child = tc.Find(".w");

            var model2 = child.ManagedObjectGet("KeyA") as TestModel;

            Assert(model2.Name == "Blah");
            Assert(model2.UserId == 3);

            model2 = child.ManagedObjectRemove("KeyA") as TestModel;

            Assert(model2.Name == "Blah");
            Assert(model2.UserId == 3);


        }

    }


}
