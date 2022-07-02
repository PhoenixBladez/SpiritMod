using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Placeable;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Items.Sets.CoilSet
{
	public class CoilEnergizerTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = true;
			AnimationFrameHeight = 36;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Coiled Energizer");
			AddMapEntry(new Color(50, 70, 150), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Electric;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .12f;
			g = .3f;
			b = 0.5f;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;
			int height = tile.TileFrameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Sets/CoilSet/CoilEnergizerTile_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
			if (t.TileFrameX % 54 == 0 && t.TileFrameY == 0)
				Main.spriteBatch.Draw(TextureAssets.Extra[60].Value, new Vector2(i * 16 - (int)Main.screenPosition.X - 44, j * 16 - (int)Main.screenPosition.Y - 48) + zero, null, new Color(3, 169, 252, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			if (tile.TileFrameX == 18 && tile.TileFrameY == 18)
				DoDustEffect(new Vector2(i * 16f + 8, j * 16f + 8), 74f);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 48, ModContent.ItemType<CoilEnergizerItem>());
			SoundEngine.PlaySound(SoundID.NPCHit3);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;

				if (player.active && !player.dead)
				{
					float speed = player.velocity.Length();
					if (speed < 8f && Main.rand.Next(7) == 0)
						DoDustEffect(player.MountedCenter, 46f - speed * 4.5f, 1.08f - speed * 0.13f, 2.08f - speed * 0.24f, player);
					player.AddBuff(ModContent.BuffType<OverDrive>(), 60);
				}
			}
		}

		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Electric);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .6f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
	}
}