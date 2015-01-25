using System.Text.RegularExpressions;

// parameters
var msBuildFileVerbosity = ParseEnum<BauMSBuild.Verbosity>(GetEnvVar("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = ParseEnum<BauNuGet.Verbosity>(GetEnvVar("NUGET_VERBOSITY") ?? "quiet", true);
var buildConfigurationName = GetEnvVar("CONFIGURATION_NAME") ?? "Debug";
var versionPreReleaseSuffix = GetEnvVar("VERSION_SUFFIX") ?? "adhoc";

// solution specific variables
var buildDir = new DirectoryInfo("./");
var buildPackagesDir = buildDir.Subdirectory("packages");
var repositoryDir = buildDir.Parent;
var artifactDir = repositoryDir.Subdirectory("artifacts");
var nugetOutputDir = artifactDir.Subdirectory("nuget");
var logsDir = artifactDir.Subdirectory("logs");
var solutionFile = repositoryDir.EnumerateFiles("*.sln").Single();

// helpers
var getVersionSuffix = fun(() => "Release".EqualsOrdinal(buildConfigurationName, true) ? string.Empty : versionPreReleaseSuffix);
var getBinaryOutputFolder = fun(() => artifactDir.Subdirectory("bin", buildConfigurationName));

// tasks
Require<Bau>()

.Task("default").DependsOn("build", "test")

.Task("release").DependsOn("set-release", "default", "pack")

.Task("set-release").Do(() => buildConfigurationName = "Release")

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
            LogFile = logsDir.Combine("build.log")
        }
    });
})

.NuGet("pack")
.DependsOn("build", "test")
.Do(nuget => {
    var specFileNames = buildDir.EnumerateFiles("*.nuspec").GetFullNames();
    var version = getBinaryOutputFolder().File("Wharrgarbl.dll").GetAssemblyName().Version.ConvertToSemVer(getVersionSuffix());
    nuget.Pack(specFileNames, r => r
        .WithOutputDirectory(nugetOutputDir.FullName)
        .WithProperty("Configuration", buildConfigurationName)
        .WithIncludeReferencedProjects()
        .WithVerbosity(nugetVerbosity)
        .WithVersion(version)
		.WithSymbols());
})

.Task("test").DependsOn("xunit")

.Xunit("xunit")
.DependsOn("build")
.Do(xunit => {
    xunit.Exe = buildPackagesDir
		.EnumerateDirectories("xunit.runners.*").Single()
        .Combine("tools/xunit.console.exe");
    xunit.Assemblies = getBinaryOutputFolder()
        .Subdirectory("tests")
        .EnumerateFiles("*.Tests.dll")
        .GetFullNames();
})
    
.Task("nuke-artifacts").Do(() => artifactDir.EnumerateDirectories().DeleteAsync(true).Wait())

.Task("create-artifact-folders").Do(() => new[] {
    artifactDir,
    nugetOutputDir,
    logsDir
}.CreateAsync().Wait())

.Run();