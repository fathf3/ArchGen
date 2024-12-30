namespace ArchGen
{
    public interface IArchitectureGenerator
    {
        void GenerateNLayerArchitecture(string basePath, string solutionName);
        void GenerateOnionArchitecture(string basePath, string solutionName);
    }
}