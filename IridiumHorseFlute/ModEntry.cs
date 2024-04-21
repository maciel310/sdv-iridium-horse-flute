using StardewModdingAPI;
using StardewValley.GameData.Tools;
using StardewValley;
using HarmonyLib;
using Iridium_Horse_Flute;

namespace IridiumHorseFlute
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ToolData data = new()
            {
                ClassName = "GenericTool",
                Name = "IridiumHorseFlute",
                DisplayName = "Iridium Horse Flute",
                Description = "Playing this flute will summon your horse and mount it.",
                SetProperties = new Dictionary<string, string> { { "InstantUse", "true" } },
                UpgradeFrom = new List<ToolUpgradeData> { 
                    new()
                    {
                        Price = 1000,
                        RequireToolId = "(O)911",
                        TradeItemId = "(O)337",
                        TradeItemAmount = 2,
                    }
                },
                Texture = helper.ModContent.GetInternalAssetName("assets/iridium-horse-flute.png").BaseName,
            };
            DataLoader.Tools(Game1.content).Add("AnthonyMaciel.IridiumHorseFlute_Flute", data);

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(Tool), nameof(Tool.beginUsing)),
                prefix: new HarmonyMethod(typeof(IridiumHorseFluteOverrides), nameof(IridiumHorseFluteOverrides.beginUsing))
            );
            
        }
    }
}