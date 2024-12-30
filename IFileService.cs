namespace ArchGen
{
    public interface IFileService
    {
        void CreateDirectory(string path);
        void WriteAllText(string path, string content);
        bool DirectoryExists(string path);
        string GetProjectNamespace(string projectPath);
    }
}