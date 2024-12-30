using System.IO;

namespace ArchGen
{
    public class FileService : IFileService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string GetProjectNamespace(string projectPath)
        {
            var projectName = new DirectoryInfo(projectPath).Name;
            return projectName.Substring(0, projectName.IndexOf('.'));
        }
    }
}