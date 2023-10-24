using Domain.Dtos;
using FluentValidation;

namespace Domain.Validators;

public class MessageDtoValidator : AbstractValidator<MessageDto>
{
    public MessageDtoValidator()
    {
        RuleFor(message => message.Content)
            .Length(15, 20)
            .Must(HaveMinimumLowercaseCharacters)
            .WithMessage("A mensagem deve conter no mínimo 2 caracteres minúsculos.")
            .Must(HaveMinimumUppercaseCharacters)
            .WithMessage("A mensagem deve conter no mínimo 5 caracteres maiúsculos.")
            .Must(HaveMinimumRepeatedCharacters)
            .WithMessage("A mensagem deve conter no mínimo 4 caracteres repetidos.")
            .Must(HaveMinimumSpecialCharacters)
            .WithMessage("A mensagem deve conter no mínimo 2 caracteres especiais.");
    }

    private static bool HaveMinimumLowercaseCharacters(string message)
    {
        return message.Count(char.IsLower) >= 2;
    }

    private static bool HaveMinimumUppercaseCharacters(string message)
    {
        return message.Count(char.IsUpper) >= 5;
    }

    private static bool HaveMinimumRepeatedCharacters(string message)
    {
        return message.GroupBy(c => c).Any(group => group.Count() >= 4);
    }

    private static bool HaveMinimumSpecialCharacters(string message)
    {
        var specialCharacterCount = message.Count(c => "!@#$%&_".Contains(c));
        return specialCharacterCount >= 2;
    }
}