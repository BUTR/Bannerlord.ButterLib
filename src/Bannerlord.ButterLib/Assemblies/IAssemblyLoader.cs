namespace Bannerlord.ButterLib.Assemblies
{
    public interface IAssemblyLoader
    {
        void LoadFile(string path);
        bool LoadFileAndTest(string path);
    }
}