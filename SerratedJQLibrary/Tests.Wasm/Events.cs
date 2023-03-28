using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasm;

namespace Tests.Wasm
{

    public class Events_Click : JQTest
    {
        public override void Run()
        {
            tc.Append(JQueryBox.FromHtml("<div class='w'>Click This</div>"));
            var child = tc.Find(".w");
            child.OnClick += Child_OnClick;
            
            // TODO: Autoamtically trigger the event from JQuery

        }

        private void Child_OnClick(JQueryBox sender, dynamic e)
        {
            string eventName = e.type;
            sender.Append(JQueryBox.FromHtml($"<span> Clicked{e}</span>"));
            Assert(eventName == "click");
        }
    }




}
