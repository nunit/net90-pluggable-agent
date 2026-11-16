// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.IO;
using System.Reflection;
#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif
using NUnit.Framework;

namespace NUnit.Engine.Agents
{
    /// <summary>Checks the launcher path without starting a process or requiring an installed agent.</summary>
    public sealed class AgentPathTests
    {
        [Test]
        public void AgentPathHasNoRelativeDirectorySegments()
        {
            string path = ReadAgentPath(typeof(Net90AgentLauncher));
            Assert.That(path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }),
                Has.None.EqualTo(".."));
            Assert.That(Path.IsPathRooted(path), Is.True);
        }

#if NET8_0_OR_GREATER
        /// <summary>A real extended-path launcher must name its existing sibling agent without dot segments.</summary>
        [Test, Platform("Win")]
        public void ExtendedLauncherLocationNamesTheExistingSiblingAgent()
        {
            string root = Path.Combine(Path.GetTempPath(), "NUnitAgentPathTests", Guid.NewGuid().ToString("N"),
                new string('a', 80), new string('b', 80), new string('c', 80));
            string launcherDirectory = Path.Combine(root, "net10.0");
            string agentDirectory = Path.Combine(root, "agent");
            Directory.CreateDirectory(launcherDirectory);
            Directory.CreateDirectory(agentDirectory);
            string source = typeof(Net90AgentLauncher).Assembly.Location;
            string launcher = Path.Combine(launcherDirectory, Path.GetFileName(source));
            File.Copy(source, launcher);
            // This inert file proves path resolution. It is never loaded or executed.
            File.WriteAllText(Path.Combine(agentDirectory, "nunit-agent-net90.dll"), "inert path-test sentinel");
            TestContext.Out.WriteLine("Retained launcher path probe: " + root);
            var context = new AssemblyLoadContext(Guid.NewGuid().ToString("N"), isCollectible: true);
            try
            {
                var assembly = context.LoadFromAssemblyPath(@"\\?\" + launcher);
                Assert.That(assembly.Location, Does.StartWith(@"\\?\"));
                Assert.That(assembly.Location.Length, Is.GreaterThan(260));
                string agent = ReadAgentPath(assembly.GetType(typeof(Net90AgentLauncher).FullName!)!);
                Assert.That(File.Exists(agent), Is.True, "Extended names must not contain unresolved parent segments.");
                Assert.That(Path.GetFileName(agent), Is.EqualTo("nunit-agent-net90.dll"));
            }
            finally
            {
                // Retain files: unloading a collectible context is not synchronous.
                context.Unload();
            }
        }
#endif

        /// <summary>A root cannot contain the launcher's required sibling agent directory.</summary>
        [Test]
        public void RejectsLauncherAtFileSystemRoot()
        {
            string directory = Path.GetPathRoot(TestContext.CurrentContext.TestDirectory)!;
            var error = PathError(directory);
            Assert.That(error, Is.TypeOf<DirectoryNotFoundException>());
            Assert.That(error.Message, Does.Contain("parent directory").And.Contain(directory));
        }

        /// <summary>A nonexistent parent must be reported instead of producing a guessed path.</summary>
        [Test]
        public void RejectsMissingParentDirectory()
        {
            string parent = Path.Combine(Path.GetTempPath(), "NUnitMissingAgentParent", Guid.NewGuid().ToString("N"));
            var error = PathError(Path.Combine(parent, "launcher"));
            Assert.That(error, Is.TypeOf<DirectoryNotFoundException>());
            Assert.That(error.Message, Does.Contain("parent directory").And.Contain(parent));
        }

        /// <summary>An incomplete package identifies its missing agent directory.</summary>
        [Test]
        public void RejectsMissingAgentDirectory()
        {
            string root = Path.Combine(Path.GetTempPath(), "NUnitAgentLayoutTests", Guid.NewGuid().ToString("N"));
            string launcher = Path.Combine(root, "launcher");
            Directory.CreateDirectory(launcher);
            try
            {
                var error = PathError(launcher);
                Assert.That(error, Is.TypeOf<DirectoryNotFoundException>());
                Assert.That(error.Message, Does.Contain("agent directory").And.Contain(Path.Combine(root, "agent")));
            }
            finally
            {
                Directory.Delete(launcher);
                Directory.Delete(root);
            }
        }

        /// <summary>An incomplete agent directory identifies the exact missing assembly.</summary>
        [Test]
        public void RejectsMissingAgentAssembly()
        {
            string root = Path.Combine(Path.GetTempPath(), "NUnitAgentLayoutTests", Guid.NewGuid().ToString("N"));
            string launcher = Path.Combine(root, "launcher");
            string agent = Path.Combine(root, "agent");
            Directory.CreateDirectory(launcher);
            Directory.CreateDirectory(agent);
            try
            {
                var error = PathError(launcher);
                Assert.That(error, Is.TypeOf<FileNotFoundException>());
                string expected = Path.Combine(agent, "nunit-agent-net90.dll");
                Assert.That(((FileNotFoundException)error).FileName, Is.EqualTo(expected));
                Assert.That(error.Message, Does.Contain(expected));
            }
            finally
            {
                Directory.Delete(agent);
                Directory.Delete(launcher);
                Directory.Delete(root);
            }
        }

        private static Exception PathError(string launcherDirectory)
        {
            Action action = () =>
            {
                typeof(Net90AgentLauncher).GetMethod("GetAgentPath", BindingFlags.NonPublic | BindingFlags.Static)!
                    .Invoke(null, new object[] { launcherDirectory });
            };
            return Assert.Throws<TargetInvocationException>(action)!.InnerException!;
        }

        private static string ReadAgentPath(Type launcher) =>
            (string)launcher.GetProperty("AgentPath", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
    }
}
