using System;
using System.Reactive;
using ReactiveUI;

namespace SpectrumTest.ViewModels
{
    public class ConfirmationViewModel : ReactiveObject
    {
        public ConfirmationViewModel()
        {
        }

        public ReactiveCommand<Unit, Unit> SignInCommand { get; set; }
    }
}
