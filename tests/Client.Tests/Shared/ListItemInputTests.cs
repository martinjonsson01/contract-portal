﻿using System.Threading.Tasks;

using Client.Shared.Lists;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Tests.Shared;

public class ListItemInputTests : UITestFixture
{
    public ListItemInputTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public async Task Input_DoesNotTriggerValueChanged_WhenValueIsSameAsPreviousAsync()
    {
        // Arrange
        const string item = "Item text";

        bool called = false;
        void OnValueChanged((string, int) tuple) => called = true;

        void ParameterBuilder(ComponentParameterCollectionBuilder<ListItemInput<string>> parameters) =>
            parameters.Add(property => property.Item, item)
                      .Add(property => property.ValueChanged, OnValueChanged)
                      .AddCascadingValue(new EditContext(string.Empty));

        IRenderedComponent<ListItemInput<string>>
            cut = Context.RenderComponent<ListItemInput<string>>(ParameterBuilder);
        cut.Render();

        const string listItemInput = ".list-item-input";
        cut.WaitForElement(listItemInput);

        // Act
        await cut.Find(listItemInput).ChangeAsync(new ChangeEventArgs { Value = item, }).ConfigureAwait(false);

        // Assert
        cut.WaitForAssertion(() => called.Should().BeFalse());
    }

    [Fact]
    public void Render_DoesNotThrow_WhenItemIsNull()
    {
        // Arrange
        static void ParameterBuilder(ComponentParameterCollectionBuilder<ListItemInput<string>> parameters) =>
            parameters.Add(property => property.Item, null)
                      .AddCascadingValue(new EditContext(string.Empty));

        IRenderedComponent<ListItemInput<string>>
            cut = Context.RenderComponent<ListItemInput<string>>(ParameterBuilder);

        // Act
        Action render = () => cut.Render();

        // Assert
        cut.WaitForAssertion(() => render.Should().NotThrow());
    }
}
