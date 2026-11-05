using FluentValidation;
using BloomShop.Models;

namespace BloomShop.Validators;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    public ContactViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required / Ім'я обов'язкове")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters / Мінімум 2 символи")
            .MaximumLength(100).WithMessage("Name is too long / Занадто довге ім'я");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required / Email обов'язковий")
            .EmailAddress().WithMessage("Invalid email format / Некоректний email");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required / Повідомлення обов'язкове")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters / Мінімум 10 символів")
            .MaximumLength(2000).WithMessage("Message is too long / Занадто довге повідомлення");
    }
}
