using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
	public class JumpPadTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileSolid[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jump Pad");
			dustType = -3;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		float alphaCounter;
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			alphaCounter += 0.04f;
			float sineAdd = (float)Math.Sin(alphaCounter) + 3;
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if(Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Tile t = Main.tile[i, j];
			if(t.frameX % 54 == 0 && t.frameY == 0) {
				Main.spriteBatch.Draw(Main.extraTexture[60], new Vector2(i * 16 - (int)Main.screenPosition.X - 44, ((j - 1) * 16) - (int)Main.screenPosition.Y - 50) + zero, null, new Color((int)(2.5f * sineAdd), (int)(5f * sineAdd), (int)(6f * sineAdd), 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Main.PlaySound(SoundID.NPCHit, i * 16, j * 16, 4);
			{
				//Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
				Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<JumpPadItem>());
			}
		}
		/*public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
            Player player = Main.LocalPlayer;
            if (closer)
            {
                int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
                if (distance1 < 36)
                {
                    player.velocity.Y -= 4f;
                }
            }
        }*/
		public override void FloorVisuals(Player player)
		{
			player.velocity.Y -= 14f;
			Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, player.Center);
			float speed = player.velocity.Length();
			for(int i = 0; i < 12; i++) {
				DoDustEffect(player.MountedCenter, 46f - speed * 4.5f, 1.08f - speed * 0.13f, 2.08f - speed * 0.24f, player);
			}
		}
		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .6f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
	}
}
