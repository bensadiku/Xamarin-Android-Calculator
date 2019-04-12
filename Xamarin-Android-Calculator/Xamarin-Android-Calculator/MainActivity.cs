using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System;

namespace Xamarin_Android_Calculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView calculatorText;

        private string[] number = new string[2];
        private string @operator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view);

        }


        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text)){
                AddDigitOrDecimalPoint(button.Text);
            }else if ("÷×+-".Contains(button.Text))
            {
                AddOperator(button.Text);
            }else if("=" == button.Text)
            {
                Calculate();
            }
            else
            {
                Erase();
            }
        }

        private void Erase()
        {
            number[0] = number[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        private void Calculate(string newOperator=null)
        {
            double? result = null;
            double? first = number[0] == null ? null : (double?)double.Parse(number[0]);
            double? second = number[1] == null ? null : (double?)double.Parse(number[1]);

            switch (@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }

            if(result != null)
            {
                number[0] = result.ToString();
                @operator = newOperator;
                number[1] = null;
                    UpdateCalculatorText();
            }
        }

        private void AddOperator(string value)
        {
           if(number[1] != null)
            {
                Calculate(value);
                return;
            }

            @operator = value;
            UpdateCalculatorText();
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;
            if (value == "." && number[index].Contains("."))
            {
                return;
            }
            number[index] += value;

            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => calculatorText.Text = $"{number[0]} {@operator} {number[1]}";

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}