namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

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

        public static Task<DirectoryInfo> CreateAsync(this DirectoryInfo directory)
        {
            if (Directory.Exists(directory.FullName))
            {
                return Task.FromResult(directory);
            }

            var taskSource = new TaskCompletionSource<DirectoryInfo>();
            var watcher = new FileSystemWatcher(directory.Parent.FullName, directory.Name);
            FileSystemEventHandler handler = null;
            handler = (_, e) =>
            {
                if (e.Name == directory.Name)
                {
                    taskSource.TrySetResult(new DirectoryInfo(directory.FullName));
                    watcher.Created -= handler;
                    watcher.Dispose();
                    directory.Refresh();
                }
            };
            watcher.Created += handler;
            watcher.EnableRaisingEvents = true;

            directory.Create();

            return taskSource.Task;
        }

        public static Task<DirectoryInfo[]> CreateAsync(this IEnumerable<DirectoryInfo> directories)
        {
            return Task.WhenAll(directories.Select(x => x.CreateAsync()));
        }

        public static Task<DirectoryInfo> DeleteAsync(this DirectoryInfo directory, bool recursive = false)
        {
            if (!Directory.Exists(directory.FullName))
            {
                return Task.FromResult(directory);
            }

            var taskSource = new TaskCompletionSource<DirectoryInfo>();
            var watcher = new FileSystemWatcher(directory.Parent.FullName, directory.Name);
            FileSystemEventHandler handler = null;
            handler = (_, e) =>
            {
                if (e.Name == directory.Name) // TODO: try to create a test that causes this to be false
                {
                    taskSource.TrySetResult(new DirectoryInfo(directory.FullName));
                    watcher.Deleted -= handler;
                    watcher.Dispose();
                }
            };
            watcher.Deleted += handler;
            watcher.EnableRaisingEvents = true;

            directory.Delete(recursive);

            return taskSource.Task;
        }

        public static Task<DirectoryInfo[]> DeleteAsync(this IEnumerable<DirectoryInfo> directories, bool recursive = false)
        {
            return Task.WhenAll(directories.Select(x => x.DeleteAsync(recursive)));
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
