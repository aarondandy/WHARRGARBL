namespace Wharrgarbl.Tests.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.CoreExtensions;
    using Xunit;

    public static class IOExtensionsFacts
    {
        [Fact]
        public static void can_construct_directory_info()
        {
            var expected = new DirectoryInfo("./");

            var actual = "./".ToDirectoryInfo();

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void can_construct_file_info()
        {
            var expected = new FileInfo("./README.md");

            var actual = "./README.md".ToFileInfo();

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void can_ref_sub_dir()
        {
            var expected = new DirectoryInfo("subDir");
            var di = new DirectoryInfo("./");

            var actual = di.Subdirectory("subDir");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void can_ref_file_in_dir()
        {
            var expected = new FileInfo("README.md");
            var di = new DirectoryInfo("./");

            var actual = di.File("README.md");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void can_ref_multi_part_sub_dir()
        {
            var expected = new DirectoryInfo("subDir1/subDir2");
            var di = new DirectoryInfo("./");

            var actual = di.Subdirectory("subDir1", "subDir2");

            actual.FullName.Should().Be(expected.FullName);
        }

        [Fact]
        public static void can_ref_multi_part_file_in_dir()
        {
            var expected = new FileInfo("./subDir/README.md");
            var di = new DirectoryInfo("./");

            var actual = di.File("subDir", "README.md");

            actual.FullName.Should().Be(expected.FullName);
        }
    }
}
