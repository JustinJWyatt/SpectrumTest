
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ClassLibrary2.ViewModels;
using ReactiveUI;

namespace SpectrumTest
{
    [Activity(Label = "ConfirmationActivity")]
    public class ConfirmationActivity : ReactiveActivity, IViewFor<ConfirmationViewModel>
    {
        private Button SigninButton { get; set; }

        ConfirmationViewModel _ViewModel;
        public ConfirmationViewModel ViewModel
        {
            get { return _ViewModel; }
            set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConfirmationViewModel)value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_confirmation);

            SigninButton = FindViewById<Button>(Resource.Id.confirmation_signin);

            ViewModel = new ConfirmationViewModel();

            ViewModel.SignInCommand = ReactiveCommand.Create(SignIn);

            this.BindCommand(this.ViewModel, vm => vm.SignInCommand, v => v.SigninButton);
        }

        public override void OnBackPressed()
        {
            SignIn();
        }

        private void SignIn()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}
