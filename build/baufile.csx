// parameters
var msBuildFileVerbosity = (BauMSBuild.Verbosity)Enum.Parse(typeof(BauMSBuild.Verbosity), Environment.GetEnvironmentVariable("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = (BauNuGet.Verbosity)Enum.Parse(typeof(BauNuGet.Verbosity), Environment.GetEnvironmentVariable("NUGET_VERBOSITY") ?? "quiet", true);
var buildConfigurationName = Environment.GetEnvironmentVariable("CONFIGURATION_NAME") ?? "Debug";

// solution specific variables
var buildDir = new DirectoryInfo("./");
var buildPackagesDir = new DirectoryInfo("packages");
var artifactDir = new DirectoryInfo("../artifacts");
var nugetOutputDir = new DirectoryInfo(Path.Combine(artifactDir.FullName, "nuget"));
var logsDir = new DirectoryInfo(Path.Combine(artifactDir.FullName, "logs"));
var repositoryDir = new DirectoryInfo("../");
var solutionFile = repositoryDir.EnumerateFiles("*.sln").Single();

var versionParts = File.ReadAllText(Path.Combine(repositoryDir.FullName, "src/Wharrgarbl/Properties/AssemblyInfo.cs"))
    .Split(new[] { "AssemblyVersion(\"" }, 2, StringSplitOptions.None)
    .ElementAt(1)
    .Split(new[] { '"' })
    .First()
    .Split(new[] { '.' })
    .Take(3);
var version = string.Join(".", versionParts);

// helpers
Func<string> getVersionSuffix = () => "Release".Equals(buildConfigurationName, StringComparison.OrdinalIgnoreCase)
        ? string.Empty
        : (Environment.GetEnvironmentVariable("VERSION_SUFFIX") ?? "-adhoc");
Func<DirectoryInfo> getBinaryOutputFolder = () => new DirectoryInfo(Path.Combine(artifactDir.FullName, "bin", buildConfigurationName));

// tasks
Require<Bau>()

.Task("default").DependsOn("build", "test")

.Task("release").DependsOn("set-release", "default", "pack")

.Task("set-release").Do(() => {buildConfigurationName = "Release";})

.NuGet("restore").Do(nuget => nuget.Restore(solutionFile.FullName))

.MSBuild("build")
.DependsOn("restore", "create-artifact-folders")
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
.DependsOn("build")
.Do(nuget => nuget.Pack(
    Directory.EnumerateFiles(buildDir.FullName, "*.nuspec"),
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
    //xunit.Args = "-parallel none";
    xunit.Assemblies = Directory.EnumerateFiles(Path.Combine(getBinaryOutputFolder().FullName,"tests"), "*.Tests.dll");
    Console.WriteLine(Path.Combine(getBinaryOutputFolder().FullName,"tests"));
})

.Task("create-artifact-folders").Do(() => {
    var dirsToCreate = new[] {
        artifactDir,
        nugetOutputDir,
        logsDir
    }.Where(di => !di.Exists);
    foreach(var di in dirsToCreate) {
        di.Create();
    }
    System.Threading.Thread.Sleep(100);
})

.Run();