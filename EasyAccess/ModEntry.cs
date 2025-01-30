using LeFauxMods.Common.Utilities;
using LeFauxMods.EasyAccess.Models;
using LeFauxMods.EasyAccess.Services;
using StardewModdingAPI.Events;
using StardewValley.Objects;

namespace LeFauxMods.EasyAccess;

/// <inheritdoc />
internal sealed class ModEntry : Mod
{
    /// <inheritdoc />
    public override void Entry(IModHelper helper)
    {
        // Init
        I18n.Init(helper.Translation);
        ModState.Init(helper);
        Log.Init(this.Monitor, ModState.Config);
        ModPatches.Apply();

        // Events
        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
    }

    private static void CollectItems()
    {
        ModState.InAction = true;

        Game1.player.Tile.ForEachTileInRange(ModState.Config.Distance, static pos =>
        {
            var hasObj = Game1.currentLocation.Objects.TryGetValue(pos, out var obj);
            var hasTerrain = Game1.currentLocation.terrainFeatures.TryGetValue(pos, out var terrain);
            var largeTerrain = Game1.currentLocation.getLargeTerrainFeatureAt((int)pos.X, (int)pos.Y);

            foreach (var feature in ModState.Config.CollectionTypes)
            {
                switch (feature)
                {
                    case CollectionType.DigSpots when
                        hasObj &&
                        obj.QualifiedItemId == "(O)590":
                        Game1.currentLocation.digUpArtifactSpot((int)pos.X, (int)pos.Y, Game1.player);
                        Game1.currentLocation.makeHoeDirt(pos, true);
                        Game1.currentLocation.Objects.Remove(pos);

                        return true;

                    case CollectionType.Forage when
                        hasObj &&
                        obj.IsSpawnedObject &&
                        obj.isForage() &&
                        Game1.tryToCheckAt(pos, Game1.player):

                    case CollectionType.Machines when
                        hasObj &&
                        obj.HasContextTag("machine_output") &&
                        obj.checkForAction(Game1.player):

                    case CollectionType.Terrain when
                        hasTerrain &&
                        terrain.performUseAction(pos):

                    case CollectionType.Terrain when
                        largeTerrain?.performUseAction(pos) == true:

                    case CollectionType.Terrain when
                        hasObj &&
                        obj is IndoorPot pot &&
                        pot.hoeDirt.Value is { } potTerrain &&
                        potTerrain.performUseAction(pos):

                        return true;

                    case CollectionType.Terrain when
                        hasObj &&
                        obj is IndoorPot pot &&
                        pot.bush.Value is { } bush &&
                        bush.performUseAction(pos):

                        return true;
                }
            }

            return true;
        });

        ModState.InAction = false;
    }

    private static void DispenseItems() =>
        Game1.player.Tile.ForEachTileInRange(ModState.Config.Distance, static pos =>
        {
            if (Game1.currentLocation.Objects.TryGetValue(pos, out var obj) &&
                obj.HasContextTag("machine_output") &&
                obj.AttemptAutoLoad(Game1.player.Items, Game1.player))
            {
                return true;
            }

            return true;
        });

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) =>
        _ = new ConfigMenu(this.Helper, this.ModManifest);

    private void OnReturnedToTitle(object? sender, ReturnedToTitleEventArgs e) =>
        this.Helper.Events.Input.ButtonsChanged -= this.OnButtonsChanged;

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e) =>
        this.Helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;

    private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (!Context.IsPlayerFree)
        {
            return;
        }

        if (ModState.Config.CollectItems.JustPressed())
        {
            this.Helper.Input.SuppressActiveKeybinds(ModState.Config.CollectItems);
            CollectItems();
            return;
        }

        if (Game1.player.CurrentItem is not null && ModState.Config.DispenseItems.JustPressed())
        {
            DispenseItems();
            this.Helper.Input.SuppressActiveKeybinds(ModState.Config.DispenseItems);
        }
    }
}