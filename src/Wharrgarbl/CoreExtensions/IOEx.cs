namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.IO;

    public static class IOEx
    {
        public static DirectoryInfo ToDirectoryInfo(this string path)
        {
            return new DirectoryInfo(path);
        }

        public static FileInfo ToFileInfo(this string path)
        {
            return new FileInfo(path);
        }

        public static DirectoryInfo Subdirectory(this DirectoryInfo directory, string relativePath)
        {
            var combined = Combine(directory, relativePath);
            return new DirectoryInfo(combined);
        }

        public static DirectoryInfo Subdirectory(this DirectoryInfo directory, params string[] relativePaths)
        {
            var combined = Combine(directory, relativePaths);
            return new DirectoryInfo(combined);
        }

        public static FileInfo File(this DirectoryInfo directory, string relativePath)
        {
            var combined = Combine(directory, relativePath);
            return new FileInfo(combined);
        }

        public static FileInfo File(this DirectoryInfo directory, params string[] relativePaths)
        {
            var combined = Combine(directory, relativePaths);
            return new FileInfo(combined);
        }

        private static string Combine(this DirectoryInfo directory, string relativePath)
        {
            return Path.Combine(directory.FullName, relativePath);
        }

        private static string Combine(this DirectoryInfo directory, params string[] relativePaths)
        {
            return Path.Combine(BuildPathParts(directory, relativePaths));
        }

        private static string[] BuildPathParts(DirectoryInfo directory, params string[] relativePaths)
        {
            var pathPaths = new string[relativePaths.Length + 1];
            pathPaths[0] = directory.FullName;
            Array.Copy(relativePaths, 0, pathPaths, 1, relativePaths.Length);
            return pathPaths;
        }
    }
}
