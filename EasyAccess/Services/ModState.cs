using LeFauxMods.Common.Services;
using StardewModdingAPI.Utilities;

namespace LeFauxMods.EasyAccess.Services;

/// <summary>Responsible for managing state.</summary>
internal sealed class ModState
{
    private static ModState? Instance;

    private readonly ConfigHelper<ModConfig> configHelper;

    private readonly PerScreen<bool> inAction = new();

    private ModState(IModHelper helper) => this.configHelper = new ConfigHelper<ModConfig>(helper);

    public static ModConfig Config => Instance!.configHelper.Config;

    public static ConfigHelper<ModConfig> ConfigHelper => Instance!.configHelper;

    public static bool InAction
    {
        get => Instance!.inAction.Value;
        set => Instance!.inAction.Value = value;
    }

    public static void Init(IModHelper helper) => Instance ??= new ModState(helper);
}