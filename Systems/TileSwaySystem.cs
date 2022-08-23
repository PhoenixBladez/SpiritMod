using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Systems
{
	internal class TileSwaySystem : ModSystem
	{
		public double TreeWindCounter { get; private set; }
		public double GrassWindCounter { get; private set; }

		public override void PreUpdateWorld()
		{
			if (!Main.dedServ)
			{
				double num = Math.Abs(Main.WindForVisuals);
				num = Utils.GetLerpValue(0.08f, 1.2f, (float)num, clamped: true);
				TreeWindCounter += 0.0041666666666666666 + 0.0041666666666666666 * num * 2.0;
				GrassWindCounter += 0.0055555555555555558 + 0.0055555555555555558 * num * 4.0;
			}
		}

		internal static void DrawGrassSway(SpriteBatch batch, string texture, int i, int j, Color color) => DrawGrassSway(batch, ModContent.Request<Texture2D>(texture).Value, i, j, color);

		internal static void DrawGrassSway(SpriteBatch batch, Texture2D texture, int i, int j, Color color)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			Vector2 pos = new Vector2(i * 16, j * 16) - Main.screenPosition + zero;

			float rot = ModContent.GetInstance<TileSwaySystem>().GetGrassSway(i, j, ref pos);
			Vector2 orig = GrassOrigin(i, j);

			batch.Draw(texture, pos + new Vector2(8, 16), new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 22), color, rot, orig, 1f, SpriteEffects.None, 0f);
		}

		internal float GetGrassSway(int i, int j, ref Vector2 position)
		{
			Tile tile = Main.tile[i, j];

			float rotation = Main.instance.TilesRenderer.GetWindCycle(i, j, GrassWindCounter);
			if (!WallID.Sets.AllowsWind[tile.WallType])
				rotation = 0f;
			if (!WorldGen.InAPlaceWithWind(i, j, 1, 1))
				rotation = 0f;
			rotation += Main.instance.TilesRenderer.GetWindGridPush(i, j, 20, 0.35f);

			position.X += rotation;
			position.Y += Math.Abs(rotation);
			return rotation * 0.1f;
		}

		internal static Vector2 GrassOrigin(int i, int j)
		{
			short _ = 0;
			Tile tile = Main.tile[i, j];
			Main.instance.TilesRenderer.GetTileDrawData(i, j, tile, tile.TileType, ref _, ref _, out var tileWidth, out var _, out var tileTop, out var halfBrickHeight, out var _, out var _, out var _, out var _, out var _, out var _);
			return new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop));
		}
	}
}
