using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class SynthwaveHead : ModTile
	{
		public virtual int FrameModY => 0;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 18 };
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(75, 139, 166));
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = -1;
			AdjTiles = new int[]{ TileID.LunarMonolith };
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hyperspace Bust");
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, Mod.Find<ModItem>("SynthwaveHeadItem").Type);

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));

			Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/Tiles/Furniture/SynthwaveHead_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

			spriteBatch.Draw(glow, new Vector2(i * 16, j * 16 + 2) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + FrameModY, 16, 16), colour);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override bool RightClick(int i, int j)
		{
			SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
			HitWire(i, j);
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = Mod.Find<ModItem>("SynthwaveHeadItem").Type;
		}

		public override void HitWire(int i, int j)
		{
			int x = i - (Main.tile[i, j].TileFrameX / 18) % 3;
			int y = j - (Main.tile[i, j].TileFrameY / 18) % 4;
			for (int l = x; l < x + 3; l++)
			{
				for (int m = y; m < y + 4; m++)
				{
					Tile tile = Main.tile[l, m];
					if (tile.HasTile)
					{
						if (tile.TileType == ModContent.TileType<SynthwaveHeadActive>())
							tile.TileType = (ushort)ModContent.TileType<SynthwaveHead>();
						else
							tile.TileType = (ushort)ModContent.TileType<SynthwaveHeadActive>();
					}
				}
			}

			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
		}
	}

	public class SynthwaveHeadActive : SynthwaveHead
	{
		public override int FrameModY => 74;
		public override string Texture => $"SpiritMod/Tiles/Furniture/{nameof(SynthwaveHead)}";

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			base.PostDraw(i, j, spriteBatch);

			if (Main.rand.NextBool(50))
			{
				int index3 = Dust.NewDust(new Vector2((i * 16), (float)(j * 16) - 20), 16, 16, DustID.Electric, 0.0f, 0f, 150, new Color(), 0.5f);
				Main.dust[index3].fadeIn = 0.75f;
				Main.dust[index3].velocity = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1));
				Main.dust[index3].noLight = true;
				Main.dust[index3].noGravity = true;
			}
			if (Main.rand.NextBool(50))
			{
				int index3 = Dust.NewDust(new Vector2((i * 16), (float)(j * 16) - 20), 16, 16, DustID.WitherLightning, 0.0f, 0f, 150, new Color(), 0.5f);
				Main.dust[index3].fadeIn = 0.75f;
				Main.dust[index3].velocity = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1));
				Main.dust[index3].noGravity = true;
			}
		}
	}
}