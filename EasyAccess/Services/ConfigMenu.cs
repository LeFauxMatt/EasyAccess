using LeFauxMods.Common.Services;
using LeFauxMods.EasyAccess.Models;

namespace LeFauxMods.EasyAccess.Services;

/// <inheritdoc />
internal sealed class ConfigMenu(IModHelper helper, IManifest manifest)
    : BaseConfigMenu<ModConfig>(helper, manifest)
{
    /// <inheritdoc />
    protected override ModConfig Config => ModState.ConfigHelper.Temp;

    /// <inheritdoc />
    protected override ConfigHelper<ModConfig> ConfigHelper => ModState.ConfigHelper;

    /// <inheritdoc />
    protected internal override void SetupOptions()
    {
        this.Api.AddKeybindList(
            this.Manifest,
            () => this.Config.CollectItems,
            value => this.Config.CollectItems = value,
            I18n.ConfigOption_CollectItems_Name,
            I18n.ConfigOption_CollectItems_Description);

        this.Api.AddKeybindList(
            this.Manifest,
            () => this.Config.DispenseItems,
            value => this.Config.DispenseItems = value,
            I18n.ConfigOption_DispenseItems_Name,
            I18n.ConfigOption_DispenseItems_Description);

        this.Api.AddNumberOption(
            this.Manifest,
            () => this.Config.Distance,
            value => this.Config.Distance = value,
            I18n.ConfigOption_Distance_Name,
            I18n.ConfigOption_Distance_Description);

        this.Api.AddBoolOption(
            this.Manifest,
            () => this.Config.CollectionTypes.Contains(CollectionType.DigSpots),
            value =>
            {
                if (value)
                {
                    this.Config.CollectionTypes.Add(CollectionType.DigSpots);
                }
                else
                {
                    this.Config.CollectionTypes.Remove(CollectionType.DigSpots);
                }
            },
            I18n.ConfigOption_CollectDigSpots_Name,
            I18n.ConfigOption_CollectDigSpots_Description);

        this.Api.AddBoolOption(
            this.Manifest,
            () => this.Config.CollectionTypes.Contains(CollectionType.Forage),
            value =>
            {
                if (value)
                {
                    this.Config.CollectionTypes.Add(CollectionType.Forage);
                }
                else
                {
                    this.Config.CollectionTypes.Remove(CollectionType.Forage);
                }
            },
            I18n.ConfigOption_CollectForage_Name,
            I18n.ConfigOption_CollectForage_Description);

        this.Api.AddBoolOption(
            this.Manifest,
            () => this.Config.CollectionTypes.Contains(CollectionType.Machines),
            value =>
            {
                if (value)
                {
                    this.Config.CollectionTypes.Add(CollectionType.Machines);
                }
                else
                {
                    this.Config.CollectionTypes.Remove(CollectionType.Machines);
                }
            },
            I18n.ConfigOption_CollectMachines_Name,
            I18n.ConfigOption_CollectMachines_Description);

        this.Api.AddBoolOption(
            this.Manifest,
            () => this.Config.CollectionTypes.Contains(CollectionType.Terrain),
            value =>
            {
                if (value)
                {
                    this.Config.CollectionTypes.Add(CollectionType.Terrain);
                }
                else
                {
                    this.Config.CollectionTypes.Remove(CollectionType.Terrain);
                }
            },
            I18n.ConfigOption_CollectTerrain_Name,
            I18n.ConfigOption_CollectTerrain_Description);
    }
}