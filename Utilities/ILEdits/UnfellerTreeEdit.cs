using log4net;
using MonoMod.Cil;
using SpiritMod.Items.Sets.ToolsMisc.Evergreen;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.ILEdits
{
	class UnfellerTreeEdit : ILEdit
	{
		public override void Load(Mod mod) => IL.Terraria.Player.ItemCheck += Player_ItemCheck;

		/// This IL edit is used to allow the unfeller of evergreens to autoplant saplings when trees are destroyed
		/// Sadly tML doesn't provide tile destruction info by default in a way feasible in MP, so we have to resort to this
		///
		/// THESE ARE THE IL INSTRUCTIONS WE'LL HARASS AND WHAT IT'LL LOOK LIKE WHEN WE'RE DONE
		///   IL_af0c: ldloc 352    <----- WE WILL FIRST FIND AND JUMP TO THIS INSTRUCTION
		///   IL_af10: nop
		///   IL_af11: nop
		///   IL_af12: callvirt instance void Terraria.HitTile::Clear(int32)  <----- NEXT WE JUMP HERE
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO PUSH THE TYPE OF THE TILE THE PLAYER JUST MINED]
		///   IL_af17: ldsfld int32 Terraria.Player::tileTargetX
		///   IL_af1c: ldsfld int32 Terraria.Player::tileTargetY
		///   IL_af21: ldc.i4.0
		///   IL_af22: ldc.i4.0
		///   IL_af23: ldc.i4.0
		///   IL_af24: call void Terraria.WorldGen::KillTile(int32, int32, bool, bool, bool)
		///   IL_XXXX: [WE RUN OUR OWN CODE HERE TO PLANT THE SAPLING]
		private static void Player_ItemCheck(ILContext il)
		{
			// Get an ILCursor and a logger to report errors if we find any
			ILCursor cursor = new ILCursor(il);
			ILog logger = ModContent.GetInstance<SpiritMod>().Logger;

			// Here we try to make the ILCursor jump to instruction IL_af0c (ldloc 352)
			// The instruction "ldloc 352" occurs 3 times before the one we want at IL_af0c
			// We will ask the cursor 4 times to go to the next specified instruction (in this case go to ldloc 352, 4 times)
			// If it can't find it, we stop IL editing and log an error
			for (int c = 0; c < 4; c++)
			{
				if (!cursor.TryGotoNext(i => i.MatchLdloc(352)))
				{
					logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: First jump failed");
					return;
				}
			}

			// At this point we reached the instruction we want
			// Now we make the cursor jump again to the HitTile.Clear call at IL_af12
			// Same error handling as before
			if (!cursor.TryGotoNext(i => i.MatchCallvirt<HitTile>("Clear")))
			{
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Second jump failed");
				return;
			}

			// We increment the cursor index by 1 to skip over the HitTile.Clear instruction
			// Then we emit a delegate to push the type of the targetted tile onto the stack
			cursor.Index++;
			cursor.EmitDelegate<Func<ushort>>(() => Main.tile[Player.tileTargetX, Player.tileTargetY].TileType);

			// Now we jump for the final time to the WorldGen.KillTile call at IL_af24
			if (!cursor.TryGotoNext(i => i.MatchCall<WorldGen>("KillTile")))
			{
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Final jump failed");
				return;
			}

			// Increment cursor index by 1 because we want to insert after the WorldGen.KillTile call
			cursor.Index++;

			// time for the fun stuff b a y b e e
			// We still have the type of the tile that got destroyed earlier pushed onto the stack
			// Now we use it by emitting a delegate that consumes this value from the stack, and we execute our own code
			// (Mother nature is happy with you)
			cursor.EmitDelegate<Action<ushort>>(type =>
			{
				// Item that was used to kill the tile
				Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];

				// If the tile that got destroyed isn't a tree, or if the item type isn't the unfeller of evergreens, we stop and return
				if (!(type == TileID.Trees || type == TileID.PalmTree) || item.type != ModContent.ItemType<UnfellerOfEvergreens>())
					return;

				// We will keep moving this tile down until we hit the tile the tree was sitting on (which will be the first solid tile we find)
				int currentX = Player.tileTargetX;
				int currentY = Player.tileTargetY;
				Tile currentTile = Framing.GetTileSafely(currentX, currentY);

				while (!currentTile.HasTile || !Main.tileSolid[currentTile.TileType])
					currentTile = Framing.GetTileSafely(currentX, ++currentY);

				// Now we finish up and plant a sapling above the tile we hit
				WorldGen.PlaceTile(currentX, currentY - 1, TileID.Saplings);
			});
		}
	}
}
