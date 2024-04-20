using StardewValley;
using SObject = StardewValley.Object;
using StardewValley.Characters;

namespace Iridium_Horse_Flute
{
    internal class IridiumHorseFluteOverrides
    {
        public static bool beginUsing(Tool __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
        {
            if (!IsIridiumHorseFlute(__instance)) return true;

            // Only process action if this is for the current player.
            if (who == null || !Game1.player.Equals(who)) return false;

            Game1.toolAnimationDone(who);
            who.CanMove = true;
            who.canReleaseTool = false;
            who.UsingTool = false;

            var flute = ItemRegistry.Create<SObject>("(O)911");
            flute.performUseAction(location);

            var horse = Utility.findHorseForPlayer(who.UniqueMultiplayerID);
            if (WillWarpHorse(horse, who))
            {
                DelayedAction.functionAfterDelay(() => horse.checkAction(who, location), 1700);
            }

            __result = false;

            return false;
        }

        private static bool WillWarpHorse(Horse horse, Farmer who)
        {
            // These are the same checks that the standard Horse Flute performs before warping the horse.
            return horse != null
                && (Math.Abs(who.Tile.X - horse.Tile.X) > 1
                    || Math.Abs(who.Tile.Y - horse.Tile.Y) > 1)
                && Utility.GetHorseWarpRestrictionsForFarmer(who) == Utility.HorseWarpRestrictions.None;
        }

        private static bool IsIridiumHorseFlute(Item tool)
        {
            return tool.QualifiedItemId == "(T)AnthonyMaciel.IridiumHorseFlute_Flute";
        }
    }
}
