using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Wasm
{

	// Client side validation demo using Luhn check digit algorithm
	public class ValidationDemoPage
    {
        public static void Init()
        {
            Console.WriteLine("Luhn Page WASM Executed.");

			var input = JQueryBox.Select("#cardInput");
            input.OnInput += Input_OnInput;// subscribe to HTML DOM input event on the #cardInput textbox, fire on each keystroke


        }

		// Event handler, called by JQuery event handler
        private static void Input_OnInput(JQueryBox sender, object e)
        {
			bool isValid = LuhnIsValid(sender.Value);
			if (isValid)
			{
				sender.AddClass("is-valid");
				sender.RemoveClass("is-invalid");
			}
			else
			{
				sender.AddClass("is-invalid");
				sender.RemoveClass("is-valid");
			}
		}

        public static bool LuhnIsValid(string creditCardNumber) 
		{
			if (creditCardNumber.Any(c => !char.IsDigit(c)))
				return false;

			List<int> digits = creditCardNumber.Select(c => int.Parse(c.ToString())).Reverse().ToList();
			digits.RemoveAt(0);

			var multiplied = digits
				.Select((d, i) =>
				{
					if ((i + 1) % 2 == 0)
					{
						return d;
					}
					else
					{

						if (d * 2 > 9)
						{
							return d * 2 - 9;
						}
						else
						{
							return d * 2;
						}
					}
				}
				);

			var sum = multiplied.Sum();
			int checkDigit = (10 - (sum % 10)) % 10;

			return int.Parse(creditCardNumber.Last().ToString()) == checkDigit;
		}


	}
}
