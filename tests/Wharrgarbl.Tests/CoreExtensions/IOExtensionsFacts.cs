namespace Wharrgarbl.Tests.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.CoreExtensions;
    using Xunit;

    public static class IOExtensionsFacts
    {
        [Fact]
        public static void construct_directory_info()
        {
            var expected = new DirectoryInfo("./");

            var actual = "./".ToDirectoryInfo();

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void construct_file_info()
        {
            var expected = new FileInfo("./README.md");

            var actual = "./README.md".ToFileInfo();

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void ref_sub_dir()
        {
            var expected = new DirectoryInfo("subDir");
            var di = new DirectoryInfo("./");

            var actual = di.Subdirectory("subDir");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void ref_file_in_dir()
        {
            var expected = new FileInfo("README.md");
            var di = new DirectoryInfo("./");

            var actual = di.File("README.md");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void ref_multi_part_sub_dir()
        {
            var expected = new DirectoryInfo("subDir1/subDir2");
            var di = new DirectoryInfo("./");

            var actual = di.Subdirectory("subDir1", "subDir2");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void ref_multi_part_file_in_dir()
        {
            var expected = new FileInfo("./subDir/README.md");
            var di = new DirectoryInfo("./");

            var actual = di.File("subDir", "README.md");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void get_names_from_multiple_files()
        {
            var files = new[] { "a.txt".ToFileInfo(), "b.txt".ToFileInfo() };

            var results = files.GetFullNames();

            results.ShouldBeEquivalentTo(new[] { new FileInfo("a.txt").FullName, new FileInfo("b.txt").FullName });
        }

        [Fact]
        public static void get_names_from_multiple_directories()
        {
            var files = new[] { "a.txt".ToDirectoryInfo(), "b.txt".ToDirectoryInfo() };

            var results = files.GetFullNames();

            results.ShouldBeEquivalentTo(new[] { new DirectoryInfo("a.txt").FullName, new DirectoryInfo("b.txt").FullName });
        }

        [Fact]
        public static void wait_for_single_directory_create()
        {
            var di = DeleteAndUpdate(GetFileName("subDir").ToDirectoryInfo());

            try
            {
                var createResult = di.CreateAsync().Result;

                Directory.Exists(createResult.FullName).Should().BeTrue();
                createResult.Exists.Should().BeTrue();
                createResult.FullName.Should().Be(di.FullName);
            }
            finally
            {
                DeleteAndUpdate(di);
            }
        }

        [Fact]
        public static void wait_for_single_directory_delete()
        {
            var di = DeleteAndUpdate(GetFileName("subDir").ToDirectoryInfo());
            di = CreateAndUpdate(di);
            try
            {
                var deleteResult = di.DeleteAsync().Result;

                Directory.Exists(deleteResult.FullName).Should().BeFalse();
                deleteResult.Exists.Should().BeFalse();
                deleteResult.FullName.Should().Be(di.FullName);
            }
            finally
            {
                DeleteAndUpdate(di);
            }
        }

        [Fact]
        public static void wait_for_many_directory_creates()
        {
            var subDirName = GetFileName("subDir");
            var dirInfos = Enumerable.Range(1, 10)
                .Select(n => subDirName + "-" + n)
                .Select(name => name.ToDirectoryInfo());

            dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            Thread.Sleep(100);

            try
            {
                var startTime = DateTime.UtcNow;
                var createResults = dirInfos.CreateAsync().Result;
                var endTime = DateTime.UtcNow;
                Console.WriteLine("Create many: {0}ms", (endTime - startTime).TotalMilliseconds);

                createResults.All(di => Directory.Exists(di.FullName)).Should().BeTrue();
                createResults.All(di => di.Exists).Should().BeTrue();
                createResults.GetFullNames().Should().BeEquivalentTo(dirInfos.GetFullNames());
            }
            finally
            {
                dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            }
        }

        [Fact]
        public static void wait_for_many_directory_deletes()
        {
            var subDirName = GetFileName("subDir");
            var dirInfos = Enumerable.Range(1, 10)
                .Select(n => subDirName + "-" + n)
                .Select(name => name.ToDirectoryInfo());

            dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            Thread.Sleep(100);
            dirInfos.ForEach(di => CreateAndUpdate(di, false));
            Thread.Sleep(100);

            try
            {
                var startTime = DateTime.UtcNow;
                var createResults = dirInfos.DeleteAsync().Result;
                var endTime = DateTime.UtcNow;
                Console.WriteLine("Delete many: {0}ms", (endTime - startTime).TotalMilliseconds);

                createResults.All(di => Directory.Exists(di.FullName)).Should().BeFalse();
                createResults.All(di => di.Exists).Should().BeFalse();
                createResults.GetFullNames().Should().BeEquivalentTo(dirInfos.GetFullNames());
            }
            finally
            {
                dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            }
        }

        [Fact]
        public static void wait_for_many_directory_deletes_with_files()
        {
            var subDirName = GetFileName("subDir");
            var dirInfos = Enumerable.Range(1, 10)
                .Select(n => subDirName + "-" + n)
                .Select(name => name.ToDirectoryInfo());

            dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            Thread.Sleep(100);

            dirInfos.ForEach(di => CreateAndUpdate(di, false));
            Thread.Sleep(100);

            dirInfos.AsParallel().ForAll(di =>
            {
                var guidValue = GetFileName();
                var textFileName = di.File(guidValue + ".txt").FullName;
                File.WriteAllText(textFileName, guidValue);
            });

            try
            {
                var startTime = DateTime.UtcNow;
                var createResults = dirInfos.DeleteAsync(true).Result;
                var endTime = DateTime.UtcNow;
                Console.WriteLine("Delete many recursive: {0}ms", (endTime - startTime).TotalMilliseconds);

                createResults.All(di => Directory.Exists(di.FullName)).Should().BeFalse();
                createResults.All(di => di.Exists).Should().BeFalse();
                createResults.GetFullNames().Should().BeEquivalentTo(dirInfos.GetFullNames());
            }
            finally
            {
                dirInfos.ForEach(di => DeleteAndUpdate(di, false));
            }
        }

        private static DirectoryInfo DeleteAndUpdate(DirectoryInfo di, bool sleep = true)
        {
            if (Directory.Exists(di.FullName))
            {
                if (sleep)
                {
                    di.Delete(true);
                    Thread.Sleep(100);
                    Directory.Exists(di.FullName).Should().BeFalse();
                    di.Refresh();
                    di.Exists.Should().BeFalse();
                }
                else
                {
                    return di.DeleteAsync(true).Result;
                }
            }

            return di;
        }

        private static DirectoryInfo CreateAndUpdate(DirectoryInfo di, bool sleep = true)
        {
            if (!Directory.Exists(di.FullName))
            {
                if (sleep)
                {
                    di.Create();
                    Thread.Sleep(100);
                    Directory.Exists(di.FullName).Should().BeTrue();
                    di.Refresh();
                    di.Exists.Should().BeTrue();
                }
                else
                {
                    return di.CreateAsync().Result;
                }
            }

            return di;
        }

        private static string GetFileName(string prefix = "")
        {
            var guidValue = Guid.NewGuid().ToString("D");
            return prefix + guidValue;
        }
    }
}
