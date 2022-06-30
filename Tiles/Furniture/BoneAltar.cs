﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Reach;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class BoneAltar : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);

			DustType = DustID.Torch;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bone Altar");
			AddMapEntry(Colors.RarityAmber, name);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .46f;
			g = .32f;
			b = .1f;
		}

		public override bool IsTileDangerous(int i, int j, Player player) => true;

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Furniture/BoneAltar_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				if (Main.rand.Next(20) == 0)
					Dust.NewDust(new Vector2(i * 16, j * 16 - 10), 0, 16, DustID.Torch, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			for (int a = 0; a < 30; a++)
			{
				Vector2 offset = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * new Vector2(40f, 40f);
				int index1 = Dust.NewDust(new Vector2(i * 16, j * 16 - 30) + offset, 0, 0, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index1].velocity = Vector2.Zero;
			}

			if (!NPC.AnyNPCs(ModContent.NPCType<ForestWraith>()))
			{
				Main.NewText("You have disturbed the ancient Nature Spirits!", 0, 170, 60);
				NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16 - 300, ModContent.NPCType<ForestWraith>(), 0, 2, 1, 0, 0, Main.myPlayer);
			}
		}
	}
}