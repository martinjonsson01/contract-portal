
dotnet clean
dotnet build
dotnet test --no-build tests/Application.Tests -p:CollectCoverage=true    -p:Threshold=80 -p:ThresholdType=branch -p:CoverletOutput=../../CoverageResults/                                                                                        -p:ExcludeByFile=\"**/Program.cs\" -p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\" -p:Include=\"[Application]*,[Domain]*\"
dotnet test --no-build tests/Domain.Tests -p:CollectCoverage=true         -p:Threshold=80 -p:ThresholdType=branch -p:CoverletOutput=../../CoverageResults/ -p:MergeWith="../../CoverageResults/coverage.json"                                     -p:ExcludeByFile=\"**/Program.cs\" -p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\" -p:Include=\"[Domain]*\"
dotnet test --no-build tests/Infrastructure.Tests -p:CollectCoverage=true -p:Threshold=80 -p:ThresholdType=branch -p:CoverletOutput=../../CoverageResults/ -p:MergeWith="../../CoverageResults/coverage.json"                                     -p:ExcludeByFile=\"**/Program.cs\" -p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\" -p:Include=\"[Infrastructure]*,[Domain]*\"
dotnet test --no-build tests/Presentation.Tests -p:CollectCoverage=true   -p:Threshold=80 -p:ThresholdType=branch -p:CoverletOutput=../../CoverageResults/ -p:MergeWith="../../CoverageResults/coverage.json" -p:CoverletOutputFormat="opencover" -p:ExcludeByFile=\"**/Program.cs\" -p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\" -p:Include=\"[Client]*,[Server]*,[Domain]*\"

dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"CoverageResults/coverage.opencover.xml" -targetdir:"Coverage/"
start Coverage/index.html
