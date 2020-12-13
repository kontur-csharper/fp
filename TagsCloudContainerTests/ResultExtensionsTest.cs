using FluentAssertions;
using NUnit.Framework;
using ResultOf;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class ResultExtensionsTest
    {
        [Test]
        public void Validate_IfCalledFromFailResult_ReturnThisResult()
        {
            var failResult = Result.Fail<int>("error message");

            failResult.Validate(num => num % 2 == 0, "other error").Should()
                .Be(failResult);
        }

        [Test]
        public void Validate_IfValueIsNotValid_ReturnFailResultWithGivenError()
        {
            var resultObj = Result.Ok(5);

            resultObj.Validate(num => num % 2 == 0, "num error").Error
                .Should().Be("num error");
        }

        [Test]
        public void Validate_IfValueIsValid_ReturnThisResult()
        {
            var resultObj = Result.Ok(4);

            resultObj.Validate(num => num % 2 == 0, "num error")
                .Should().Be(resultObj);
        }
    }
}