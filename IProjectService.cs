namespace ArchGen
{
    public interface IProjectService
    {
        void CreateSolution(string basePath, string solutionName);
        void CreateProject(string projectPath, string projectType);
        void AddProjectToSolution(string basePath, string projectPath);
        void AddPackageReference(string projectPath, string projectName, string packageName, string version);
        void AddProjectReference(string projectPath, string projectName, string referenceProjectPath);
    }
}