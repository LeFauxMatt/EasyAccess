using LeFauxMods.Common.Integrations.GenericModConfigMenu;
using LeFauxMods.Common.Services;
using LeFauxMods.EasyAccess.Models;

namespace LeFauxMods.EasyAccess.Services;

/// <summary>Responsible for handling the mod configuration menu.</summary>
internal sealed class ConfigMenu
{
    private readonly IGenericModConfigMenuApi api = null!;
    private readonly GenericModConfigMenuIntegration gmcm;
    private readonly IManifest manifest;

    public ConfigMenu(IModHelper helper, IManifest manifest)
    {
        this.manifest = manifest;
        this.gmcm = new GenericModConfigMenuIntegration(manifest, helper.ModRegistry);
        if (!this.gmcm.IsLoaded)
        {
            return;
        }

        this.api = this.gmcm.Api;
        this.SetupMenu();
    }

    private static ModConfig Config => ModState.ConfigHelper.Temp;

    private static ConfigHelper<ModConfig> ConfigHelper => ModState.ConfigHelper;

    private void SetupMenu()
    {
        this.gmcm.Register(ConfigHelper.Reset, ConfigHelper.Save);

        this.api.AddKeybindList(
            this.manifest,
            static () => Config.CollectItems,
            static value => Config.CollectItems = value,
            I18n.ConfigOption_CollectItems_Name,
            I18n.ConfigOption_CollectItems_Description);

        this.api.AddKeybindList(
            this.manifest,
            static () => Config.DispenseItems,
            static value => Config.DispenseItems = value,
            I18n.ConfigOption_DispenseItems_Name,
            I18n.ConfigOption_DispenseItems_Description);

        this.api.AddNumberOption(
            this.manifest,
            static () => Config.Distance,
            static value => Config.Distance = value,
            I18n.ConfigOption_Distance_Name,
            I18n.ConfigOption_Distance_Description);

        this.api.AddBoolOption(
            this.manifest,
            static () => Config.CollectionTypes.Contains(CollectionType.DigSpots),
            static value =>
            {
                if (value)
                {
                    Config.CollectionTypes.Add(CollectionType.DigSpots);
                }
                else
                {
                    Config.CollectionTypes.Remove(CollectionType.DigSpots);
                }
            },
            I18n.ConfigOption_CollectDigSpots_Name,
            I18n.ConfigOption_CollectDigSpots_Description);

        this.api.AddBoolOption(
            this.manifest,
            static () => Config.CollectionTypes.Contains(CollectionType.Forage),
            static value =>
            {
                if (value)
                {
                    Config.CollectionTypes.Add(CollectionType.Forage);
                }
                else
                {
                    Config.CollectionTypes.Remove(CollectionType.Forage);
                }
            },
            I18n.ConfigOption_CollectForage_Name,
            I18n.ConfigOption_CollectForage_Description);

        this.api.AddBoolOption(
            this.manifest,
            static () => Config.CollectionTypes.Contains(CollectionType.Machines),
            static value =>
            {
                if (value)
                {
                    Config.CollectionTypes.Add(CollectionType.Machines);
                }
                else
                {
                    Config.CollectionTypes.Remove(CollectionType.Machines);
                }
            },
            I18n.ConfigOption_CollectMachines_Name,
            I18n.ConfigOption_CollectMachines_Description);

        this.api.AddBoolOption(
            this.manifest,
            static () => Config.CollectionTypes.Contains(CollectionType.Terrain),
            static value =>
            {
                if (value)
                {
                    Config.CollectionTypes.Add(CollectionType.Terrain);
                }
                else
                {
                    Config.CollectionTypes.Remove(CollectionType.Terrain);
                }
            },
            I18n.ConfigOption_CollectTerrain_Name,
            I18n.ConfigOption_CollectTerrain_Description);
    }
}