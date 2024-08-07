using StardewModdingAPI;
using StardewValley.GameData.Tools;
using StardewValley;
using HarmonyLib;
using Iridium_Horse_Flute;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;

namespace IridiumHorseFlute
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private readonly string _toolsSpriteName = "Mods/AnthonyMaciel.IridiumHorseFlute/Tools";

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += this.Edit;

            helper.GameContent.Load<Texture2D>(_toolsSpriteName);

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(Tool), nameof(Tool.beginUsing)),
                prefix: new HarmonyMethod(typeof(IridiumHorseFluteOverrides), nameof(IridiumHorseFluteOverrides.beginUsing))
            );
        }

        public void Edit(object sender, AssetRequestedEventArgs args)
        {
            if (args.Name.IsEquivalentTo(_toolsSpriteName))
            {
                args.LoadFromModFile<Texture2D>("assets/iridium-horse-flute.png", AssetLoadPriority.High);
            }
            else if (args.Name.IsEquivalentTo("Data/Tools"))
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
                    Texture = _toolsSpriteName,
                };
                args.Edit(asset =>
                {
                    var toolDatas = asset.AsDictionary<string, ToolData>().Data;

                    toolDatas["AnthonyMaciel.IridiumHorseFlute_Flute"] = data;
                });
            }
        }
    }
}