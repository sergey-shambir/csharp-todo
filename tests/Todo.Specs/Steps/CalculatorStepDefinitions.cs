using TechTalk.SpecFlow;
using Xunit;

namespace Todo.Specs.Steps;

[Binding]
public sealed class CalculatorStepDefinitions(ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;
    private readonly List<int> _numbers = [];
    private int _result = 0;

    [Given("я ввёл число (.*)")]
    public void ПустьЯВвёлЧисло(int number)
    {
        _numbers.Add(number);
    }

    [When("я нажал сложить")]
    public void КогдаЯНажалСложить()
    {
        _result = _numbers.Sum();
    }

    [When("я нажал умножить")]
    public void КогдаЯНажалУмножить()
    {
        _result = 1;
        foreach (int number in _numbers)
        {
            _result *= number;
        }
    }

    [Then("результат равен (.*)")]
    public void ТогдаРезультатРавен(int expected)
    {
        Assert.Equal(expected, _result);
    }
}
