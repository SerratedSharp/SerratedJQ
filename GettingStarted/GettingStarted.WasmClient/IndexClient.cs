using SerratedSharp.SerratedJQ.Plain;

namespace GettingStarted.WasmClient
{

    // Client side validation demo using Luhn check digit algorithm
    public class IndexClient
    {
        public static void Init()
        {
            Console.WriteLine("Index Page WASM Executed.");

            JQueryPlain.Select(".container main").Append(JQueryPlain.ParseHtmlAsJQuery(
                """
                <div class="row">
                    <div class="col">
                        <label for="cardInput">Credit Card #, type 4111111111111111 for a valid check digit.</label>
                        <input type="text" class="form-control" id="cardInput" value="">
                        <div class="valid-feedback">
                            Valid Luhn checkdigit.
                        </div>
                        <div class="invalid-feedback">
                            Invalid Luhn checkdigit.
                        </div>
                    </div>
                </div>
                """));

            var input = JQueryPlain.Select("#cardInput");
            input.OnInput += Input_OnInput;
        }

        private static void Input_OnInput(JQueryPlainObject sender, object e)
        {
            string cardNumber = sender.Val<string>().Trim();
            // Check Luhn validation
            bool isValid = string.IsNullOrWhiteSpace(cardNumber) == false && LuhnIsValid(cardNumber);
            // Adding validation class
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

        // Luhn check digit algorithm
        public static bool LuhnIsValid(string creditCardNumber)
        {
            if (creditCardNumber.Any(c => !char.IsDigit(c)))
                return false;

            List<int> digits = creditCardNumber.Select(c => int.Parse(c.ToString())).Reverse().ToList();
            digits.RemoveAt(0);

            var multiplied = digits
                .Select((d, i) => {
                    if ((i + 1) % 2 == 0) {
                        return d;
                    }
                    else 
                    {
                        if (d * 2 > 9) {
                            return d * 2 - 9;
                        }
                        else {
                            return d * 2;
                        }
                    }
                });

            var sum = multiplied.Sum();
            int checkDigit = (10 - (sum % 10)) % 10;
            return int.Parse(creditCardNumber.Last().ToString()) == checkDigit;
        }

    }
}
