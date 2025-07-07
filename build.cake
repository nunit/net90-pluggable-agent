// Load the recipe
#load nuget:?package=NUnit.Cake.Recipe&version=1.5.0
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../NUnit.Cake.Recipe/recipe/*.cake

BuildSettings.Initialize
(
    context: Context,
    title: "Net90PluggableAgent",
    solutionFile: "net90-pluggable-agent.sln",
    unitTests: "**/*.tests.exe",
    githubOwner: "NUnit",
    githubRepository: "net90-pluggable-agent",
    exemptFiles: new[] { "ProcessUtils.cs" }
);

var PackageTests = new PackageTest[] {
    new PackageTest(1, "NetCore31PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET Core 3.1",
        Arguments = "testdata/netcoreapp3.1/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net60PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 6.0",
        Arguments = "testdata/net6.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net70PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 7.0",
        Arguments = "testdata/net7.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net80PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 8.0",
        Arguments = "testdata/net8.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net90PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 9.0",
        Arguments = "testdata/net9.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "AspNetCore60Test")
    {
        Description = "Run test using AspNetCore targeting .NET 6.0",
        Arguments = "testdata/net6.0/aspnetcore-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll") }
        }
    },
    new PackageTest(1, "AspNetCore80Test")
    {
        Description = "Run test using AspNetCore targeting .NET 8.0",
        Arguments = "testdata/net8.0/aspnetcore-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll") }
        }
    },
    new PackageTest(1, "AspNetCore90Test")
    {
        Description = "Run test using AspNetCore targeting .NET 9.0",
        Arguments = "testdata/net9.0/aspnetcore-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll") }
        }
    },
    new PackageTest(1, "Net60WindowsFormsTest")
    {
        Description = "Run test using windows forms under .NET 6.0",
        Arguments = "testdata/net6.0-windows/windows-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-test.dll") }
        }
    },
    new PackageTest(1, "Net80WindowsFormsTest")
    {
        Description = "Run test using windows forms under .NET 8.0",
        Arguments = "testdata/net8.0-windows/windows-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-test.dll") }
        }
    },
    new PackageTest(1, "Net90WindowsFormsTest")
    {
        Description = "Run test using windows forms under .NET 9.0",
        Arguments = "testdata/net9.0-windows/windows-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-test.dll") }
        }
    }
};

BuildSettings.Packages.Add(new NuGetPackage(
    id: "NUnit.Extension.Net90PluggableAgent",
    source: BuildSettings.NuGetDirectory + "Net90PluggableAgent.nuspec",
    checks: new PackageCheck[]
    {
        HasFiles("LICENSE.txt", "README.md", "nunit_256.png"),
        HasDirectory("tools").WithFiles(
            "nunit-agent-launcher-net90.dll", "nunit.engine.api.dll"),
        HasDirectory("tools/agent").WithFiles(
            "nunit-agent-net90.dll", "nunit.engine.api.dll", "nunit.common.dll", 
            "nunit.extensibility.api.dll", "nunit.extensibility.dll", "nunit.agent.core.dll",
            "TestCentric.Metadata.dll", "Microsoft.Extensions.DependencyModel.dll")
    },
    testRunner: new AgentRunner(BuildSettings.NuGetTestDirectory + "NUnit.Extension.Net90PluggableAgent." + BuildSettings.PackageVersion + "/tools/agent/nunit-agent-net90.dll"),
    tests: PackageTests
    ));

BuildSettings.Packages.Add(new ChocolateyPackage(
    "nunit-extension-net90-pluggable-agent",
    source: BuildSettings.ChocolateyDirectory + "net90-pluggable-agent.nuspec",
    checks: new PackageCheck[]
    {
        HasDirectory("tools").WithFiles(
            "LICENSE.txt", "README.md", "nunit_256.png", "VERIFICATION.txt",
            "nunit-agent-launcher-net90.dll", "nunit.engine.api.dll"),
        HasDirectory("tools/agent").WithFiles(
            "nunit-agent-net90.dll", "nunit.engine.api.dll", "nunit.common.dll",
            "nunit.extensibility.api.dll", "nunit.extensibility.dll", "nunit.agent.core.dll",
            "TestCentric.Metadata.dll", "Microsoft.Extensions.DependencyModel.dll")
    },
    testRunner: new AgentRunner(BuildSettings.ChocolateyTestDirectory + "nunit-extension-net90-pluggable-agent." + BuildSettings.PackageVersion + "/tools/agent/nunit-agent-net90.dll"),
    tests: PackageTests));

Task("PublishToNuGet")
    .Description("""
	Publishes packages to NuGet for an alpha, beta, rc or final release. If not,
	or if the --nopush option was used, a message is displayed.
	""")
    .Does(() =>
    {
        if (!BuildSettings.ShouldPublishToNuGet)
            Information("Nothing to publish to NuGet from this run.");
        else if (CommandLineOptions.NoPush)
            Information("NoPush option suppressing publication to NuGet");
        else
            foreach (var package in BuildSettings.Packages)
            {
                var packageName = $"{package.PackageId}.{BuildSettings.PackageVersion}.nupkg";
                var packagePath = BuildSettings.PackageDirectory + packageName;
                try
                {
                    if (package.PackageType == PackageType.NuGet)
                        NuGetPush(packagePath, new NuGetPushSettings() { ApiKey = BuildSettings.NuGetApiKey, Source = BuildSettings.NuGetPushUrl });
                }
                catch (Exception ex)
                {
                    Error(ex.Message);
                    throw;
                }
            }
    });

Task("PublishToChocolatey")
    .Description("""
	Publishes packages to Chocolatey for an alpha, beta, rc or final release.
	If not, or if the --nopush option was used, a message is displayed.
	""")
    .Does(() =>
    {
        if (!BuildSettings.ShouldPublishToChocolatey)
            Information("Nothing to publish to Chocolatey from this run.");
        else if (CommandLineOptions.NoPush)
            Information("NoPush option suppressing publication to Chocolatey");
        else
            foreach (var package in BuildSettings.Packages)
            {
                var packageName = $"{package.PackageId}.{BuildSettings.PackageVersion}.nupkg";
                var packagePath = BuildSettings.PackageDirectory + packageName;
                try
                {
                    if (package.PackageType == PackageType.Chocolatey)
                        ChocolateyPush(packagePath, new ChocolateyPushSettings() { ApiKey = BuildSettings.ChocolateyApiKey, Source = BuildSettings.ChocolateyPushUrl });
                }
                catch (Exception ex)
                {
                    Error(ex.Message);
                    throw;
                }
            }
    });
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.Run();
