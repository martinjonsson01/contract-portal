# Technical Guide
Here you will find links to different documentation sites and tutorials describing different aspects of this project.

Use this document as guidance whenever you encounter something that is new to you.

## Agile & Scrum
* [The scrum guide](https://scrumguides.org/docs/scrumguide/v2020/2020-Scrum-Guide-US.pdf)
* [Google's code review guide](https://google.github.io/eng-practices/review/reviewer/)

## C#
* [Short comparison to Java](https://nerdparadise.com/programming/csharpforjavadevs)
* [Portal with links to all of Microsoft's C# documentation](https://docs.microsoft.com/en-us/dotnet/csharp/), the following are particularily useful:
  * [Playlist of video tutorials](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oVxKLQCHpiUWun7vlJJvUiN)
  * [Pattern matching](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching)
  * [Nullable reference types tutorial](https://docs.microsoft.com/en-us/learn/modules/csharp-null-safety/)
  * [LINQ basics](https://docs.microsoft.com/en-us/dotnet/csharp/linq/query-expression-basics)
  * [Lambda expressions](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions)
  * [async / await and concurrency](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)

## Testing
* We follow [this document](https://dotnet.github.io/docfx/guideline/csharp_coding_standards#unit-tests-and-functional-tests), which in summary says:
  * Name test projects with a ".Tests" suffix
  * Name test classes with a "Test" suffix
  * Give test methods long and descriptive names
    * E.g. `PlacePiece_WithOutOfBoundsPosition_ThrowsException()`, not `PlacePieceTest()`
  * Follow AAA-structure:
    * `// Arrange // Act // Assert`
    * Important that `// Act` section is a single statement, so it only has one reason to fail
  * Use built-in assertions instead of writing complex assertion logic that asserts multiple things in a row
* Try to follow [TDD](https://www.codecademy.com/article/tdd-red-green-refactor)
* Test-writing frameworks:
  * [Unit tests](https://xunit.net/)
  * [UI testing](https://docs.microsoft.com/en-us/aspnet/core/blazor/test?view=aspnetcore-6.0)
  * [Fake data](https://github.com/bchavez/Bogus#the-great-c-example)
  * [Assertion](https://fluentassertions.com/introduction)
  * [Mocking](https://github.com/Moq/moq4/wiki/Quickstart)

## Git
* [Our Git workflow](git-workflow.md)
* [How to reverse things when you've made a mistake](https://ohshitgit.com/)
* [Full Git documentation](https://git-scm.com/docs)

## Blazor
* [Overview](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)
* [Basic tutorial](https://docs.microsoft.com/en-us/learn/modules/build-blazor-webassembly-visual-studio-code/)
* [UI Components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-6.0)
* [Project structure](https://docs.microsoft.com/en-us/aspnet/core/blazor/project-structure?view=aspnetcore-6.0)
* [Routing and navigation](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-6.0)
* [Dependency injection](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-6.0)

## ASP.NET Core web API
* [Overview](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0)
* [Basic tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code)

## Entity Framework Core
* [Basic tutorial](https://docs.microsoft.com/en-us/learn/modules/persist-data-ef-core/)
* [How to use in Blazor *server* apps](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-6.0)