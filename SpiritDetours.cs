using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using SpiritMod.Items.Tool;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class SpiritDetours
	{
		public static void Initialize()
		{
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawNPC += Main_DrawNPC;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;
			On.Terraria.Player.KeyDoubleTap += Player_KeyDoubleTap;
			On.Terraria.Main.DrawProjectiles += AddtiveCalls;
			On.Terraria.Player.ToggleInv += Player_ToggleInv;
			On.Terraria.Main.DrawInterface += DrawParticles;
			On.Terraria.Localization.LanguageManager.GetTextValue_string += LanguageManager_GetTextValue_string1;
			IL.Terraria.Player.ItemCheck += Player_ItemCheck;
		}

		private static void AddtiveCalls(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			AdditiveCallManager.DrawAdditiveCalls(Main.spriteBatch);
			orig(self);
		}

		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (!Main.dedServ)
			{
				SpiritMod.primitives.DrawTrailsProj(Main.spriteBatch);
				SpiritMod.TrailManager.DrawTrails(Main.spriteBatch);
			}
			orig(self);
		}

		private static void Main_DrawNPC(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
		{
			if (!Main.dedServ)
			{
				SpiritMod.primitives.DrawTrailsNPC(Main.spriteBatch);
			}
			orig(self, iNPCIndex, behindTiles);
		}

		private static int Projectile_NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
		{
			int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			Projectile projectile = Main.projectile[index];

			if (Main.netMode != NetmodeID.Server) SpiritMod.TrailManager.DoTrailCreation(projectile);

			return index;
		}

		private static void Player_KeyDoubleTap(On.Terraria.Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			orig(self, keyDir);
			self.GetSpiritPlayer().DoubleTapEffects(keyDir);
		}

		private static void Player_ToggleInv(On.Terraria.Player.orig_ToggleInv orig, Player self)
		{
			SpiritMod spirit = ModContent.GetInstance<SpiritMod>();

			if (spirit.BookUserInterface.CurrentState != null) {
				spirit.BookUserInterface.SetState(null);
				Main.PlaySound(SoundID.MenuClose);
				return;
			}

			orig(self);
		}

        private static string LanguageManager_GetTextValue_string1(On.Terraria.Localization.LanguageManager.orig_GetTextValue_string orig, Terraria.Localization.LanguageManager self, string key)
        {
            if (key == "GameUI.LightRain" || key == "GameUI.Rain" || key == "GameUI.HeavyRain" || key == "GameUI.Clear" || key == "GameUI.PartlyCloudy" || key == "GameUI.MostlyCloudy" || key == "GameUI.Overcast"|| key == "GameUI.Cloudy")
            {
                return SpiritMod.GetWeatherRadioText(key);
            }
            return orig(self, key);
        }

		private static void DrawParticles(On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			orig(self, gameTime);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			ParticleHandler.DrawAllParticles(Main.spriteBatch);
			Main.spriteBatch.End();
		}

		// This IL edit is used to allow the unfeller of evergreens to autoplant saplings when trees are destroyed
		// Sadly tML doesn't provide tile destruction info by default in a way feasible in MP, so we have to resort to this
		//
		// THESE ARE THE IL INSTRUCTIONS WE'LL HARASS AND WHAT IT'LL LOOK LIKE WHEN WE'RE DONE
		//   IL_af0c: ldloc 352    <----- WE WILL FIRST FIND AND JUMP TO THIS INSTRUCTION
		//   IL_af10: nop
		//   IL_af11: nop
		//   IL_af12: callvirt instance void Terraria.HitTile::Clear(int32)  <----- NEXT WE JUMP HERE
		//   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO PUSH THE TYPE OF THE TILE THE PLAYER JUST MINED]
		//   IL_af17: ldsfld int32 Terraria.Player::tileTargetX
		//   IL_af1c: ldsfld int32 Terraria.Player::tileTargetY
		//   IL_af21: ldc.i4.0
		//   IL_af22: ldc.i4.0
		//   IL_af23: ldc.i4.0
		//   IL_af24: call void Terraria.WorldGen::KillTile(int32, int32, bool, bool, bool)
		//   IL_XXXX: [WE RUN OUR OWN CODE HERE TO PLANT THE SAPLING]
		private static void Player_ItemCheck(ILContext il) {
			// Get an ILCursor and a logger to report errors if we find any
			ILCursor cursor = new ILCursor(il);
			ILog logger = ModContent.GetInstance<SpiritMod>().Logger;

			// Here we try to make the ILCursor jump to instruction IL_af0c (ldloc 352)
			// The instruction "ldloc 352" occurs 3 times before the one we want at IL_af0c
			// We will ask the cursor 4 times to go to the next specified instruction (in this case go to ldloc 352, 4 times)
			// If it can't find it, we stop IL editing and log an error
			for (int c = 0; c < 4; c++) {
				if (!cursor.TryGotoNext(i => i.MatchLdloc(352))) {
					logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: First jump failed");
					return;
				}
			}

			// At this point we reached the instruction we want
			// Now we make the cursor jump again to the HitTile.Clear call at IL_af12
			// Same error handling as before
			if (!cursor.TryGotoNext(i => i.MatchCallvirt<HitTile>("Clear"))) {
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Second jump failed");
				return;
			}

			// We increment the cursor index by 1 to skip over the HitTile.Clear instruction
			// Then we emit a delegate to push the type of the targetted tile onto the stack
			cursor.Index++;
			cursor.EmitDelegate<Func<ushort>>(() => Main.tile[Player.tileTargetX, Player.tileTargetY].type);

			// Now we jump for the final time to the WorldGen.KillTile call at IL_af24
			if (!cursor.TryGotoNext(i => i.MatchCall<WorldGen>("KillTile"))) {
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Final jump failed");
				return;
			}

			// Increment cursor index by 1 because we want to insert after the WorldGen.KillTile call
			cursor.Index++;

			// time for the fun stuff b a y b e e
			// We still have the type of the tile that got destroyed earlier pushed onto the stack
			// Now we use it by emitting a delegate that consumes this value from the stack, and we execute our own code
			// (Mother nature is happy with you)
			cursor.EmitDelegate<Action<ushort>>(type => {
				// Item that was used to kill the tile
				Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];

				// If the tile that got destroyed isn't a tree, or if the item type isn't the unfeller of evergreens, we stop and return
				if (!(type == TileID.Trees || type == TileID.PalmTree) || item.type != ModContent.ItemType<UnfellerOfEvergreens>())
					return;

				// We will keep moving this tile down until we hit the tile the tree was sitting on (which will be the first solid tile we find)
				int currentX = Player.tileTargetX;
				int currentY = Player.tileTargetY;
				Tile currentTile = Framing.GetTileSafely(currentX, currentY);

				while (!currentTile.active() || !Main.tileSolid[currentTile.type])
					currentTile = Framing.GetTileSafely(currentX, ++currentY);

				// Now we finish up and plant a sapling above the tile we hit
				WorldGen.PlaceTile(currentX, currentY - 1, TileID.Saplings);
			});
		}
	}
}
