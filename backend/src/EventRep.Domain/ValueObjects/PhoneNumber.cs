using System.Diagnostics.CodeAnalysis;
using EventRep.Domain.Common;
using FluentResults;

namespace EventRep.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public const int CanonicalLength = 12;

    private const int NationalNumberLength = 10;
    private const char RussiaCountryCode = '7';
    private const char DomesticPrefix = '8';

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
            return Result.Fail<PhoneNumber>("Номер телефона обязателен.");

        var input = rawValue.Trim();

        if (!ContainsOnlySupportedCharacters(input))
            return Result.Fail<PhoneNumber>(
                "Номер телефона содержит недопустимые символы.");

        var digits = new string(input.Where(IsAsciiDigit).ToArray());

        if (input.StartsWith('+')
            && (digits.Length != NationalNumberLength + 1
                || digits[0] != RussiaCountryCode))
        {
            return Result.Fail<PhoneNumber>(
                "Международный российский номер должен начинаться с +7.");
        }

        if (digits.Length == NationalNumberLength)
            digits = RussiaCountryCode + digits;
        else if (digits.Length == NationalNumberLength + 1 && digits[0] == DomesticPrefix)
            digits = RussiaCountryCode + digits[1..];
        else if (digits.Length != NationalNumberLength + 1 || digits[0] != RussiaCountryCode)
            return Result.Fail<PhoneNumber>(
                "Номер должен содержать 10 цифр либо начинаться с +7, 7 или 8.");

        return Result.Ok(new PhoneNumber($"+{digits}"));
    }

    public static bool TryCreate(
        string? rawValue,
        [NotNullWhen(true)] out PhoneNumber? phoneNumber)
    {
        var result = Create(rawValue);
        phoneNumber = result.IsSuccess ? result.Value : null;
        return result.IsSuccess;
    }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    private static bool ContainsOnlySupportedCharacters(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];

            if (IsAsciiDigit(character)
                || char.IsWhiteSpace(character)
                || character is '-' or '(' or ')')
            {
                continue;
            }

            if (character == '+' && index == 0)
                continue;

            return false;
        }

        return true;
    }

    private static bool IsAsciiDigit(char character) =>
        character is >= '0' and <= '9';
}
