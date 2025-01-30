using HarmonyLib;
using LeFauxMods.Common.Utilities;

namespace LeFauxMods.EasyAccess.Services;

/// <summary>Encapsulates mod patches.</summary>
internal static class ModPatches
{
    private static readonly Harmony Harmony = new(ModConstants.ModId);

    public static void Apply()
    {
        try
        {
            Log.Info("Applying patches");

            _ = Harmony.Patch(
                AccessTools.DeclaredMethod(typeof(Farmer), nameof(Farmer.isRidingHorse)),
                postfix: new HarmonyMethod(typeof(ModPatches), nameof(Farmer_isRidingHorse_postfix)));

            _ = Harmony.Patch(
                AccessTools.DeclaredMethod(typeof(Utility), nameof(Utility.tileWithinRadiusOfPlayer)),
                postfix: new HarmonyMethod(typeof(ModPatches), nameof(Utility_tileWithinRadiusOfPlayer_postfix)));
        }
        catch (Exception)
        {
            Log.WarnOnce("Failed to apply patches");
        }
    }

    private static void Farmer_isRidingHorse_postfix(ref bool __result) => __result = __result && !ModState.InAction;

    private static void Utility_tileWithinRadiusOfPlayer_postfix(ref bool __result) =>
        __result = __result || ModState.InAction;
}