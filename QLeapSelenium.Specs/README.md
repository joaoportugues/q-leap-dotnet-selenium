// replace the driver to run 
sed -i '/string browser/c\string browser = \"chrome\";' CalculatorSelenium.Specs\Drivers\BrowserDriver.cs & dotnet test --filter Category=run1

// to run tests 
dotnet test

// run by tags
dotnet test --filter Category=run

//run in parallel
edit AssemblyByInfo

// reports
dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI

cd \bin\Debug\net6.0
livingdoc test-assembly QLeapSelenium.Specs.dll -t TestExecution.json
open : bin\Debug\net6.0\LivingDoc.html