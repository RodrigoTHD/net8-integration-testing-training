using AuthAPI.IntegrationTests.Contexts;

namespace AuthAPI.IntegrationTests.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private CalculatorContext context;

        // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
        public CalculatorStepDefinitions(CalculatorContext context)
        {
            this.context = context;
        }

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            context.FirstNumber = number;
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            context.SecondNumber = number;
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            context.Result = context.FirstNumber + context.SecondNumber;
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            context.Result.Should().Be(result);
        }
    }
}
