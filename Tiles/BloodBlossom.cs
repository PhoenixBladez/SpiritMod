﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.ReachBoss;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
	public class BloodBlossom : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.AnchorBottom = new AnchorData((AnchorType)0b_11111111, 3, 0); //Any anchor is valid
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blood Blossom");
			AddMapEntry(new Color(234, 0, 0), name);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MyWorld.downedReachBoss;
		public override bool CanExplode(int i, int j) => false;

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			TileUtilities.BlockActuators(i, j);
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.frameY <= 18 && (tile.frameX <= 36 || tile.frameX >= 72))
			{
				r = 0.301f * 1.5f;
				g = 0.110f * 1.5f;
				b = 0.126f * 1.5f;
			}
		}
		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.showItemIcon = true; //Show text when hovering over this tile
			Main.LocalPlayer.showItemIcon2 = -1;// mod.ItemType("VinewrathBox");

			if (NPC.AnyNPCs(ModContent.NPCType<ReachBoss>()) || NPC.AnyNPCs(ModContent.NPCType<ReachBoss1>()))
				Main.LocalPlayer.showItemIconText = "";
			else
				Main.LocalPlayer.showItemIconText = "Disturbing this flower surely isn't a good idea...";
		}

		public override bool NewRightClick(int i, int j)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<ReachBoss>()) || NPC.AnyNPCs(ModContent.NPCType<ReachBoss1>())) //Do nothing if the boss is alive
				return false;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Main.NewText("The Vinewrath Bane has awoken!", 175, 75, 255, true);
				int npcID = NPC.NewNPC(i * 16, (j * 16) - 600, ModContent.NPCType<ReachBoss>());
				Main.npc[npcID].netUpdate2 = true;
			}
			else
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
					return false;

				SpiritMultiplayer.SpawnBossFromClient((byte)Main.LocalPlayer.whoAmI, ModContent.NPCType<ReachBoss>(), i * 16, (j * 16) - 600);
			}
			return true;
		}

		private float GetOffset() => (float)Math.Sin(Main.GlobalTime * 1.2f) * 8f;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/BloodBlossom");
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

			spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero + (Vector2.UnitY * GetOffset()), new Rectangle(tile.frameX, tile.frameY, 16, 16), Lighting.GetColor(i, j));
			return false;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

			Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/BloodBlossom_Glow");
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

			spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero + (Vector2.UnitY * GetOffset()), new Rectangle(tile.frameX, tile.frameY, 16, 16), colour);
		}
	}
}
