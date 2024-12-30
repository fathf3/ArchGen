using System;
using System.Diagnostics;
using System.IO;

namespace ArchGen
{
    public class ProjectService : IProjectService
    {
        public void CreateSolution(string basePath, string solutionName)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new sln -n {solutionName}",
                WorkingDirectory = basePath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(startInfo);
            process?.WaitForExit();
        }

        public void CreateProject(string projectPath, string projectType)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = projectType == "api" ?
                    $"new webapi -o {projectPath}" :
                    $"new classlib -o {projectPath}",
                WorkingDirectory = Path.GetDirectoryName(projectPath),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(startInfo);
            process?.WaitForExit();
        }

        public void AddProjectToSolution(string basePath, string projectPath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"sln add {projectPath}",
                WorkingDirectory = basePath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(startInfo);
            process?.WaitForExit();
        }

        public void AddPackageReference(string projectPath, string projectName, string packageName, string version)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"add \"{projectPath}/{projectName}.csproj\" package {packageName} --version {version}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(startInfo);
                process?.WaitForExit();

                if (process?.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to add package reference: {projectName} -> {packageName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding package reference: {ex.Message}");
            }
        }

        public void AddProjectReference(string projectPath, string projectName, string referenceProjectPath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"add \"{projectPath}/{projectName}.csproj\" reference \"{referenceProjectPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(startInfo);
                process?.WaitForExit();

                if (process?.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to add project reference: {projectName} -> {referenceProjectPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding project reference: {ex.Message}");
            }
        }
    }
}