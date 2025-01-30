using System.Globalization;
using System.Text;
using LeFauxMods.Common.Interface;
using LeFauxMods.Common.Models;
using LeFauxMods.EasyAccess.Models;
using StardewModdingAPI.Utilities;

namespace LeFauxMods.EasyAccess;

/// <inheritdoc cref="IModConfig{TConfig}" />
internal sealed class ModConfig : IModConfig<ModConfig>, IConfigWithLogAmount
{
    /// <summary>Gets or sets controls to collect items from producers.</summary>
    public KeybindList CollectItems { get; set; } = new(SButton.Delete);

    /// <summary>Gets or sets controls to dispense items into producers.</summary>
    public KeybindList DispenseItems { get; set; } = new(SButton.Insert);

    /// <summary>Gets or sets the distance that producers can be interacted with.</summary>
    public int Distance { get; set; } = 15;

    /// <summary>Gets or sets the features which are enabled.</summary>
    public HashSet<CollectionType> CollectionTypes { get; set; } =
        [CollectionType.DigSpots, CollectionType.Forage, CollectionType.Machines, CollectionType.Terrain];

    /// <inheritdoc />
    public LogAmount LogAmount { get; set; }

    /// <inheritdoc />
    public void CopyTo(ModConfig other)
    {
        other.LogAmount = this.LogAmount;
        other.CollectItems = this.CollectItems;
        other.DispenseItems = this.DispenseItems;
        other.Distance = this.Distance;
        other.CollectionTypes.Clear();
        other.CollectionTypes.UnionWith(this.CollectionTypes);
    }

    /// <inheritdoc />
    public string GetSummary() =>
        new StringBuilder()
            .AppendLine(CultureInfo.InvariantCulture, $"{nameof(this.CollectItems),25}: {this.CollectItems}")
            .AppendLine(CultureInfo.InvariantCulture, $"{nameof(this.DispenseItems),25}: {this.DispenseItems}")
            .AppendLine(CultureInfo.InvariantCulture, $"{nameof(this.Distance),25}: {this.Distance}")
            .AppendLine(CultureInfo.InvariantCulture,
                $"{nameof(this.CollectionTypes),25}: {string.Join(',', this.CollectionTypes)}")
            .ToString();
}