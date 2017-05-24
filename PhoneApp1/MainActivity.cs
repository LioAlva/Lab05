using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp1
{
    [Activity(Label = "PhoneApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);

            CallButton.Enabled = false;
            var TranslatedNumber = string.Empty;
            TranslateButton.Click += (object senser, System.EventArgs e) =>
            {
                var Translator = new PhoneTranslator();
                TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    CallButton.Text = "Llamar";
                    CallButton.Enabled = false;
                }
                else
                {
                    CallButton.Text = $"Llamar al {TranslatedNumber}";
                    CallButton.Enabled = true;
                }
            };

            CallButton.Click += (object sender, System.EventArgs e) =>
            {
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al numero  {TranslatedNumber}?");

                CallDialog.SetNeutralButton("Llamar", delegate {

                    var CallIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);

                    CallIntent.SetData(Android.Net.Uri.Parse($"tel:{TranslatedNumber}"));
                    StartActivity(CallIntent);
                });
                CallDialog.SetNegativeButton("Cancelar", delegate { });

                CallDialog.Show();
            };
            Validate();
        }

        private async void Validate()
        {
            var TextValidator = FindViewById<TextView>(Resource.Id.TextValidator);
            SALLab05.ServiceClient ServiceClient = new
                SALLab05.ServiceClient();

            string StudentEmail = "luisperso_015@hotmail.com";
            string Password = "Troyano12345_";

            string myDevice = Android.Provider
                .Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            SALLab05.ResultInfo Result = await
                ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);
            TextValidator.Text = ($"{Result.Status} \n{Result.Fullname}\n {Result.Token}");

        }
    }
}

