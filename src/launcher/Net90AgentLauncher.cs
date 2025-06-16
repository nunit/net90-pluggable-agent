// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

#if NETFRAMEWORK
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using NUnit.Agents;
using NUnit.Common;
using NUnit.Extensibility;

namespace NUnit.Engine.Agents
{
    [Extension]
    public class Net90AgentLauncher : LocalProcessAgentLauncher
    {
        private static readonly string LAUNCHER_DIR = AssemblyHelper.GetDirectoryName(Assembly.GetExecutingAssembly());

        protected override string AgentName => "Net90Agent";
        protected override TestAgentType AgentType => TestAgentType.LocalProcess;
        protected override FrameworkName AgentRuntime => new FrameworkName(FrameworkIdentifiers.NetCoreApp, new Version(9, 0, 0));

        protected override string AgentPath => Path.Combine(LAUNCHER_DIR, $"agent/nunit-agent-net90.dll");
    }
}
#endif
