using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using System;
using TileID = Terraria.ID.TileID;
using Terraria.ID;

namespace SpiritMod.Tiles.Block
{
	public class SpiritGrass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			AddMapEntry(new Color(0, 191, 255));
			DustType = DustID.Flare_Blue;
			ItemDrop = ModContent.ItemType<SpiritDirtItem>();
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			if (!TileObject.CanPlace(x, y, type, style, direction, out TileObject toBePlaced, false))
				return false;
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute)
				WorldGen.SquareTileFrame(x, y, true);
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.Next(40) == 0)
			{
				switch (Main.rand.Next(5))
				{
					case 0:
						PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA1").Type);
						NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA1").Type, 0, 0, -1, -1);
						break;
					case 1:
						PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA2").Type);
						NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA2").Type, 0, 0, -1, -1);
						break;
					case 2:
						PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA3").Type);
						NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA3").Type, 0, 0, -1, -1);
						break;
					case 3:
						PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA4").Type);
						NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA4").Type, 0, 0, -1, -1);
						break;

					default:
						PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA5").Type);
						NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA5").Type, 0, 0, -1, -1);
						break;
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tile2 = Framing.GetTileSafely(i, j - 1);
			if (tile.Slope == 0 && !tile.IsHalfBlock)
			{
				if (!Main.tileSolid[tile2.TileType] || !tile2.HasTile)
				{
					Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));

					Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritGrass_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
					Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

					spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), colour);
				}
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile2 = Framing.GetTileSafely(i, j - 1);
			if (!Main.tileSolid[tile2.TileType] || !tile2.HasTile)
			{
				r = 0.3f;
				g = 0.45f;
				b = 1.05f;
			}
		}
	}
}