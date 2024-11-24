using TechTalk.SpecFlow;
using Todo.Specs.Context;
using Xunit;

namespace Todo.Specs.Hooks;

[Binding]
public class TodoListHooks(TodoListContext context, ScenarioContext scenarioContext)
{
    [AfterScenario]
    public void AfterScenario()
    {
        if (scenarioContext.ScenarioInfo.Tags.Contains("negative"))
        {
            Assert.NotNull(context.LastException);
        }
        else if (context.LastException != null)
        {
            throw context.LastException;
        }
    }
}
