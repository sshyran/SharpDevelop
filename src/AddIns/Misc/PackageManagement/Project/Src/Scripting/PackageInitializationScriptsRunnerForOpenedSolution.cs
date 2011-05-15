﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.PackageManagement.Scripting
{
	public class PackageInitializationScriptsRunnerForOpenedSolution
	{
		IPackageInitializationScriptsFactory scriptsFactory;
		IPackageManagementConsoleHost consoleHost;
		
		public PackageInitializationScriptsRunnerForOpenedSolution(
			IPackageManagementProjectService projectService,
			IPackageManagementConsoleHost consoleHost)
			: this(projectService, consoleHost, new PackageInitializationScriptsFactory())
		{
		}
		
		public PackageInitializationScriptsRunnerForOpenedSolution(
			IPackageManagementProjectService projectService,
			IPackageManagementConsoleHost consoleHost,
			IPackageInitializationScriptsFactory scriptsFactory)
		{
			projectService.SolutionLoaded += SolutionLoaded;
			this.consoleHost = consoleHost;
			this.scriptsFactory = scriptsFactory;
		}
		
		void SolutionLoaded(object sender, SolutionEventArgs e)
		{
			RunPackageInitializationScripts(e.Solution);
		}
		
		void RunPackageInitializationScripts(Solution solution)
		{
			if (SolutionHasPackageInitializationScripts(solution)) {
				RunInitializePackagesCmdlet();
			}
		}
		
		bool SolutionHasPackageInitializationScripts(Solution solution)
		{
			IPackageInitializationScripts scripts = CreatePackageInitializationScripts(solution);
			return scripts.Any();
		}
		
		void RunInitializePackagesCmdlet()
		{
			string command = "Invoke-InitializePackages";
			consoleHost.ScriptingConsole.SendLine(command);
		}
		
		IPackageInitializationScripts CreatePackageInitializationScripts(Solution solution)
		{
			return scriptsFactory.CreatePackageInitializationScripts(solution, null);
		}
	}
}
