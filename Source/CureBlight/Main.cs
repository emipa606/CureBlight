using System.Reflection;
using HarmonyLib;
using Verse;

namespace CureBlight;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new Harmony("Mlie.CureBlight").PatchAll(Assembly.GetExecutingAssembly());
    }
}