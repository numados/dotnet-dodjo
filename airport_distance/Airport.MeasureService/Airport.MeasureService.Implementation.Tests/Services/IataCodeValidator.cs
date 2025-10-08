using Airport.MeasureService.Implementation.Services.Validators;

namespace Airport.MeasureService.Implementation.Tests.Services;

[TestFixture]
public class IataCodeValidatorTests
{
    [TestCase("", ExpectedResult = false)]
    [TestCase("AM", ExpectedResult = false)]
    [TestCase("AMSM", ExpectedResult = false)]
    [TestCase("AMS", ExpectedResult = true)]
    [TestCase("123", ExpectedResult = false)]
    [TestCase("AM1", ExpectedResult = false)]
    public bool IsValidIataCodeTests(string code)
    {
        return new IataCodeValidator().IsValidIataCode(code);
    }
}