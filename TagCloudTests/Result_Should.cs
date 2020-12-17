using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace ResultOf
{
    [TestFixture]
    public class Result_Should
    {
        [Test]
        public void Create_Ok()
        {
            var r = Result.Ok(42);
            r.IsSuccess.Should().BeTrue();
            r.GetValueOrThrow().Should().Be(42);
        }

        [Test]
        public void Create_Fail()
        {
            var r = Result.Fail<int>("123");

            r.IsSuccess.Should().BeFalse();
            r.Error.Should().Be("123");
        }

        [Test]
        public void ReturnsFail_FromResultOf_OnException()
        {
            var res = Result.Of<int>(() => throw new Exception("123"));

            res.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
        {
            var res = Result.Of<int>(() => { throw new Exception("123"); }, "42");

            res.Should().BeEquivalentTo(Result.Fail<int>("42"));
        }

        [Test]
        public void ReturnsOk_FromResultOf_WhenNoException()
        {
            var res = Result.Of(() => 42);

            res.Should().BeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void RunThen_WhenOk()
        {
            var res = Result.Ok(42)
                .Then(n => n + 10);
            res.Should().BeEquivalentTo(Result.Ok(52));
        }

        [Test]
        public void SkipThen_WhenFail()
        {
            var fail = Result.Fail<int>("������");
            var called = false;
            fail.Then(n =>
            {
                called = true;
                return n;
            });
            called.Should().BeFalse();
        }

        [Test]
        public void Then_ReturnsFail_OnException()
        {
            static int Continuation(int n)
            {
                throw new Exception("123");
            }

            var res = Result.Ok(42)
                .Then(Continuation);
            res.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void RunOnFail_WhenFail()
        {
            var fail = Result.Fail<int>("������");
            var errorHandler = A.Fake<Action<string>>();

            var res = fail.OnFail(errorHandler);

            A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
            res.Should().BeEquivalentTo(fail);
        }

        [Test]
        public void SkipOnFail_WhenOk()
        {
            var ok = Result.Ok(42);

            var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

            res.Should().BeEquivalentTo(ok);
        }

        [Test]
        public void RunThen_WhenOk_Scenario()
        {
            var res =
                Result.Ok("1358571172")
                    .Then(int.Parse)
                    .Then(i => Convert.ToString(i, 16))
                    .Then(hex => Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void RunThen_WhenOk_ComplexScenario()
        {
            var parsed = Result.Ok("1358571172").Then(int.Parse);
            var res = parsed
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => parsed.GetValueOrThrow() + " -> " + Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReplaceError_IfFail()
        {
            Result.Fail<None>("error")
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(Result.Fail<None>("replaced"));
        }

        [Test]
        public void ReplaceError_DoNothing_IfSuccess()
        {
            Result.Ok(42)
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void ReplaceError_DontReplace_IfCalledBeforeError()
        {
            Result.Ok(42)
                .ReplaceError(e => "replaced")
                .Then(n => Result.Fail<int>("error"))
                .Should().BeEquivalentTo(Result.Fail<int>("error"));
        }

        [Test]
        public void RefineError_AddErrorMessageBeforePreviousErrorText()
        {
            var calculation = Result.Fail<None>("No connection");
            calculation
                .RefineError("Posting results to db")
                .Should().BeEquivalentTo(Result.Fail<None>("Posting results to db. No connection"));
        }
    }
}