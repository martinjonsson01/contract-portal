using Client.Shared.Lists;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Presentation.Tests.Client.Shared;

public class ListInputTests : UITestFixture
{
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

        // Act
        cut.Find(".list-item-input").Change(new ChangeEventArgs { Value = newValue, });

        // Assert
        items.Should().Contain(newValue);
    }
}
