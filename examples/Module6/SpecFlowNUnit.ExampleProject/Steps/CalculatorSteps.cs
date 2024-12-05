using SpecFlowNUnit.ExampleProject.Contexts;

namespace SpecFlowNUnit.ExampleProject.Steps;

[Binding]
public class CalculatorSteps
{
    private readonly CalculatorContext context;

    // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
    public CalculatorSteps(CalculatorContext context)
    {
        this.context = context;
    }

    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
    [Given("the first number is (.*)")]
    //[Given("the first number is {int}")]
    public void GivenTheFirstNumberIs(int number)
    {
        context.FirstNumber = number;
    }

    [Given("the second number is {int}")]
    public void GivenTheSecondNumberIs(int number)
    {
        context.SecondNumber = number;
    }

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {
        context.Result = context.FirstNumber + context.SecondNumber;
    }

    [Then("the result should be {int}")]
    public void ThenTheResultShouldBe(int result)
    {
        context.Result.Should().Be(result);
    }
}
