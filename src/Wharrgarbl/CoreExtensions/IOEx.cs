namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

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

        public static IEnumerable<string> GetFullNames(this IEnumerable<FileSystemInfo> infos)
        {
            if (infos == null) throw new ArgumentNullException("infos");
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            return infos.Select(x => x.FullName);
        }

        public static void CreateAndWait(this DirectoryInfo directory)
        {
            var watcher = new FileSystemWatcher(directory.Parent.FullName);
            directory.Create();
            watcher.WaitForChanged(WatcherChangeTypes.Created);
        }

        public static void DeleteAndWait(this DirectoryInfo directory)
        {
            var watcher = new FileSystemWatcher(directory.FullName);
            directory.Delete();
            watcher.WaitForChanged(WatcherChangeTypes.Deleted);
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
