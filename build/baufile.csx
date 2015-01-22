using Wharrgarbl.CoreExtensions;
using Wharrgarbl.Functions.EnumFn;
using Wharrgarbl.Functions.EnvFn;
using Wharrgarbl.Functions.Fn;

// parameters
var msBuildFileVerbosity = ParseEnum<BauMSBuild.Verbosity>(GetEnvVar("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = ParseEnum<BauNuGet.Verbosity>(GetEnvVar("NUGET_VERBOSITY") ?? "quiet", true);
var buildConfigurationName = GetEnvVar("CONFIGURATION_NAME") ?? "Debug";

// solution specific variables
var buildDir = new DirectoryInfo("./");
var buildPackagesDir = buildDir.Subdirectory("packages");
var repositoryDir = buildDir.Parent;
var artifactDir = repositoryDir.Subdirectory("artifacts");
var nugetOutputDir = artifactDir.Subdirectory("nuget");
var logsDir = artifactDir.Subdirectory("logs");
var solutionFile = repositoryDir.EnumerateFiles("*.sln").Single();

var versionParts = File.ReadAllText(repositoryDir.File("src/Wharrgarbl/Properties/AssemblyInfo.cs").FullName)
    .Split(new[] { "AssemblyVersion(\"" }, 2, StringSplitOptions.None)
    .ElementAt(1)
    .Split(new[] { '"' })
    .First()
    .Split(new[] { '.' })
    .Take(3);
var version = string.Join(".", versionParts);

// helpers
var getVersionSuffix = fun(
    () => "Release".Equals(buildConfigurationName, StringComparison.OrdinalIgnoreCase)
    ? string.Empty
    : (Environment.GetEnvironmentVariable("VERSION_SUFFIX") ?? "-adhoc"));
var getBinaryOutputFolder = fun(() => new DirectoryInfo(Path.Combine(artifactDir.FullName, "bin", buildConfigurationName)));

// tasks
Require<Bau>()

.Task("default").DependsOn("build", "test")

.Task("release").DependsOn("set-release", "default", "pack")

.Task("set-release").Do(() => {buildConfigurationName = "Release";})

.NuGet("restore").Do(nuget => nuget.Restore(solutionFile.FullName))

.MSBuild("build")
.DependsOn("restore", "nuke-artifacts", "create-artifact-folders")
.Do(msb => {
    msb.Solution = solutionFile.FullName;
    msb.Targets = new[] { "Clean", "Build" };
    msb.Properties = new { Configuration = buildConfigurationName };
    msb.Verbosity = BauMSBuild.Verbosity.Minimal;
    msb.NoLogo = true;
    msb.FileLoggers.Add(new FileLogger {
        FileLoggerParameters = new FileLoggerParameters {
            PerformanceSummary = true,
            Summary = true,
            Verbosity = msBuildFileVerbosity,
            LogFile = Path.Combine(logsDir.FullName, "build.log")
        }
    });
})

.NuGet("pack")
.DependsOn("build", "test")
.Do(nuget => nuget.Pack(
    buildDir.EnumerateFiles("*.nuspec").GetFullNames(),
    r => r
        .WithOutputDirectory(nugetOutputDir.FullName)
        .WithProperty("Configuration", buildConfigurationName)
        .WithIncludeReferencedProjects()
        .WithVerbosity(nugetVerbosity)
        .WithVersion(version + getVersionSuffix())
		.WithSymbols()
))

.Task("test").DependsOn("xunit")

.Xunit("xunit")
.DependsOn("build")
.Do(xunit => {
    xunit.Exe = buildPackagesDir
		.EnumerateDirectories("xunit.runners.*").Single()
		.EnumerateDirectories("tools").Single()
        .EnumerateFiles("xunit.console.exe").Single()
        .FullName;
    xunit.Assemblies = getBinaryOutputFolder()
        .Subdirectory("tests")
        .EnumerateFiles("*.Tests.dll")
        .GetFullNames();
})
    
.Task("nuke-artifacts").Do(() => {
    artifactDir.EnumerateDirectories().DeleteAsync(true).Wait();
})

.Task("create-artifact-folders").Do(() => {
    new[] {
        artifactDir,
        nugetOutputDir,
        logsDir
    }.CreateAsync().Wait();
})

.Run();