using Android.App;
using Android.Widget;
using Android.OS;

namespace Lab07
{
    [Activity(Label = "Lab07", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView tvValidacion;
        string txtResult;
        string txtResultB;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView (Resource.Layout.Main);

            //Controles de UI:
            tvValidacion = FindViewById<TextView>(Resource.Id.tvValidacion);
            var btnValidar = FindViewById<Button>(Resource.Id.btnValidar);
            var etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            //Evento al dar click al botón:
            btnValidar.Click += (sender, e) =>
            {
                var Email = etEmail.Text;
                var Password = etPassword.Text;
                var Device = Android.Provider.Settings.Secure.GetString(ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId);

                Validar(Email, Password, Device);

                //Detectar nivel de API del dispositivo:
                //API >= 21 -> Notificación; API < 21 -> Resultado en pantalla.
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    var Builder = new Notification.Builder(this)
                        .SetContentTitle("Validacion de actividad")
                        .SetContentText(txtResultB)
                        .SetSmallIcon(Resource.Drawable.Icon);

                    Builder.SetCategory(Notification.CategoryMessage);

                    var ObjectNotification = Builder.Build();
                    var Manager = GetSystemService(Android.Content.Context.NotificationService)
                        as NotificationManager;

                    Manager.Notify(0, ObjectNotification);
                }
                else
                {
                    tvValidacion.Text = txtResult;
                }
            };
        }

        public async void Validar(string email, string password, string device)
        {
            var sc = new SALLab07.ServiceClient();
            var scResult = await sc.ValidateAsync(email, password, device);

            txtResult = $"{scResult.Status}\n{scResult.Fullname}\n{scResult.Token}";
            txtResultB = $"{scResult.Status} {scResult.Fullname} {scResult.Token}";
        }
    }
}

