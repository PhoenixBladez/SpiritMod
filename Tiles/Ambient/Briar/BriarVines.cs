using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Ambient.Briar
{
	public class BriarVines : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileCut[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = false;
			soundType = SoundID.Grass;
			DustType = DustID.Plantera_Green;

			AddMapEntry(new Color(95, 143, 65));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = 4;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Framing.GetTileSafely(i, j + 1);
			if (tile.HasTile && tile.TileType == Type) {
				WorldGen.KillTile(i, j + 1);
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			int type = -1;
			if (tileAbove.HasTile && !tileAbove.BottomSlope) {
				type = tileAbove.TileType;
			}

			if (type == ModContent.TileType<BriarGrass>() || type == Type) {
				return true;
			}

			WorldGen.KillTile(i, j);
			return true;
		}

		public override void RandomUpdate(int i, int j)
		{
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			if (WorldGen.genRand.NextBool(15) && !tileBelow.HasTile && !(tileBelow.LiquidType == LiquidID.Lava)) {
				bool placeVine = false;
				int yTest = j;
				while (yTest > j - 10) {
					Tile testTile = Framing.GetTileSafely(i, yTest);
					if (testTile.BottomSlope) {
						break;
					}
					else if (!testTile.HasTile || testTile.TileType != ModContent.TileType<BriarGrass>()) {
						yTest--;
						continue;
					}
					placeVine = true;
					break;
				}
				if (placeVine) {
					tileBelow.TileType = Type;
					tileBelow.HasTile = true;
					WorldGen.SquareTileFrame(i, j + 1, true);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
					}
				}
			}
		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) 
        {
			Tile tile = Framing.GetTileSafely(i, j);

            var source = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16); 
            Rectangle realSource = source;

            float xOff = GetOffset(i, j, tile.TileFrameX); //Sin offset.
            Vector2 drawPos = ((new Vector2(i, j)) * 16) - Main.screenPosition;

			Color col = Lighting.GetColor(i, j, Color.White); 
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

			spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/Tiles/Ambient/Briar/BriarVines", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, drawPos + zero - new Vector2(xOff, 0), realSource, new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            return false;
        }

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			Color colour = new Color (246, 194, 255) * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));

            float xOff = GetOffset(i, j, tile.TileFrameX); //Sin offset.

			Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/Tiles/Ambient/Briar/BriarVinesGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero - new Vector2(xOff, 0), new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), colour * .6f);
		}

		public float GetOffset(int i, int j, int frameX, float sOffset = 0f)
		{
			float sin = (float)Math.Sin((Main.time + (i * 24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1)) + sOffset) * 1.4f;
			if (Framing.GetTileSafely(i, j - 1).TileType != Type) //Adjusts the sine wave offset to make it look nicer when closer to ground
				sin *= 0.25f;
			else if (Framing.GetTileSafely(i, j - 2).TileType != Type)
				sin *= 0.5f;
			else if (Framing.GetTileSafely(i, j - 3).TileType != Type)
				sin *= 0.75f;

			return sin;
		}
	}
}