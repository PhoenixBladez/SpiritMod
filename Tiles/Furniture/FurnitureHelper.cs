using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;

namespace SpiritMod.Tiles.Furniture
{
	public static class FurnitureHelper
	{
		public static bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range

		public static void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info, int nextStyleHeight = 40)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
			//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

			info.TargetDirection = -1;
			if (tile.TileFrameX != 0)
				info.TargetDirection = 1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)

			// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
			// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
			info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
			info.AnchorTilePosition.Y = j;

			if (tile.TileFrameY % nextStyleHeight == 0)
				info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
		}

		public static bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) // Avoid being able to trigger it from long range
			{
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}

			return true;
		}

		public static void MouseOver(int i, int j, int itemType)
		{
			Player player = Main.LocalPlayer;

			if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) // Match condition in RightClick. Interaction should only show if clicking it does something
				return;

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = itemType;

			if (Main.tile[i, j].TileFrameX / 18 < 1)
				player.cursorItemIconReversed = true;
		}
	}
}
