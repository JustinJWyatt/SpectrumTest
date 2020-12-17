using System;
using System.Text.RegularExpressions;
using FluentValidation;
using SpectrumTest.ViewModels;

namespace SpectrumTest.Validation
{
    public class AccountValidator : AbstractValidator<AccountViewModel>
    {
        private Regex SpecialCharacterRegex => new Regex(@"^[^!@#$%^&]+$");
        private Regex LowerCaseLetterRegex => new Regex(@"(?=.*[a-z])");
        private Regex UpperCaseLetterRegex => new Regex(@"(?=.*[A-Z])");

        public AccountValidator()
        {
            RuleFor(x => x.FirstName).Must(HaveNoSpecialCharacters).WithMessage("First name should not have a special character");
            RuleFor(x => x.LastName).Must(HaveNoSpecialCharacters).WithMessage("Last name should not have a special character");
            RuleFor(x => x.Password).Must(MeetLengthRequirement).WithMessage("Password must be between 8 and 15 characters");
            RuleFor(x => x.Password).Must(ContainUpperCaseAndLowerCaseLetter).WithMessage("Password must have at least one lowercase and uppercase letter");
            RuleFor(x => x.Password).Must(HaveNoRepeatingSubstrings).WithMessage("Password must not have any repetitive sequence of characters");
            RuleFor(x => x.PhoneNumber).Must(MatchPhoneNumberFormat).WithMessage("Phone number must match format: (XXX)-XXX-XXXX");
            RuleFor(x => x.ServiceStartDate).NotNull().WithMessage("Must provide a service start date");
            RuleFor(x => x.ServiceStartDate).Must(MustHavePresentOrFutureStartDate).WithMessage("A past date is not allowed");
        }

        private bool HaveNoSpecialCharacters(string name) => SpecialCharacterRegex.IsMatch(name);
        private bool MeetLengthRequirement(string password) => password.Length >= 8 || password.Length <= 15;
        private bool ContainUpperCaseAndLowerCaseLetter(string password) => LowerCaseLetterRegex.IsMatch(password) && UpperCaseLetterRegex.IsMatch(password);
        private bool HaveNoRepeatingSubstrings(string password) => !((password + password).IndexOf(password, 1) != password.Length);
        private bool MatchPhoneNumberFormat(string phoneNumber) => true;
        private bool MustHavePresentOrFutureStartDate(DateTime? date) => date.HasValue && (DateTime.Now.Date - date.Value.Date) <= TimeSpan.FromDays(0);
    }
}
