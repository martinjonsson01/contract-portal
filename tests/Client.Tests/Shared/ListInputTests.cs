using Client.Shared.Lists;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Tests.Shared;

public class ListInputTests : UITestFixture
{
    public ListInputTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void InputTyped_UpdatesValueInList_WhenNewValueIsInput()
    {
        // Arrange
        string[] items = { "one", "two", "three", };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ListInput<string>> parameters) =>
            parameters.Add(property => property.Items, items)
                      .AddCascadingValue(new EditContext(string.Empty));

        IRenderedComponent<ListInput<string>> cut = Context.RenderComponent<ListInput<string>>(ParameterBuilder);

        const string newValue = "new value";

        string listItemInput = ".list-item-input";
        cut.WaitForElement(listItemInput);

        // Act
        cut.WaitForAssertion(() => cut.Find(listItemInput).Change(new ChangeEventArgs { Value = newValue, }));

        // Assert
        cut.WaitForAssertion(() => items.Should().Contain(newValue));
    }
}
