using System.Diagnostics;

namespace GrandHeroFarmer.Modules
{
    internal static class Helpers
    {
        public static string GetProgramVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }
    }
}