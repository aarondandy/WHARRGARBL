namespace Wharrgarbl.Tests.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.CoreExtensions;
    using Xunit;

    public static class ReflectionExtensionFacts
    {
        [Fact]
        public static void get_assembly_name_for_file_info()
        {
            var expectedAssemblyName = typeof(ReflectionEx).Assembly.GetName();
            var assemblyFileInfo = typeof(ReflectionEx).Assembly.Location.ToFileInfo();

            var result = assemblyFileInfo.GetAssemblyName();

            result.FullName.Should().Be(expectedAssemblyName.FullName);
        }

        [Fact]
        public static void get_first_three_version_parts()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.GetThreePartVersion();

            result.Should().Be("1.2.3");
        }

        [Fact]
        public static void get_sem_ver_from_version()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer();

            result.Should().Be("1.2.3");
        }

        [Fact]
        public static void get_sem_ver_from_version_with_pre_release()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer(preRelease: "alpha7");

            result.Should().Be("1.2.3-alpha7");
        }

        [Fact]
        public static void get_sem_ver_from_version_with_pre_release_and_hyphen()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer(preRelease: "-alpha7");

            result.Should().Be("1.2.3-alpha7");
        }

        [Fact]
        public static void get_sem_ver_from_version_with_metadata()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer(metadata: "build20150125");

            result.Should().Be("1.2.3+build20150125");
        }

        [Fact]
        public static void get_sem_ver_from_version_with_metadata_and_plux()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer(metadata: "+20150125");

            result.Should().Be("1.2.3+20150125");
        }

        [Fact]
        public static void get_sem_ver_from_version_with_pre_release_and_metadata()
        {
            var version = new Version(1, 2, 3, 4);

            var result = version.ConvertToSemVer(preRelease: "alpha7", metadata: "2015.01.25");

            result.Should().Be("1.2.3-alpha7+2015.01.25");
        }
    }
}
