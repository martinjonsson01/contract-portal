
dotnet test --verbosity=minimal -p:CollectCoverage=true -p:Threshold=80 -p:ThresholdType=branch -p:CoverletOutputFormat=cobertura -p:ExcludeByFile=\"**/Program.cs\" -p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\"
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"tests/Application.Tests/coverage.cobertura.xml;tests/Domain.Tests/coverage.cobertura.xml;tests/Infrastructure.Tests/coverage.cobertura.xml;tests/Presentation.Tests/coverage.cobertura.xml" -targetdir:"Coverage/"
