using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters.Algae
{
	public class BlueAlgae3 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bioluminescent Algae");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 6;
			NPC.height = 6;
			NPC.damage = 0;
			NPC.defense = 1000;
			NPC.lifeMax = 1;
			NPC.aiStyle = -1;
			NPC.npcSlots = 0;
			NPC.noGravity = false;
			NPC.alpha = 40;
			NPC.behindTiles = true;
			NPC.dontCountMe = true;
			NPC.dontTakeDamage = true;
		}
		public float num42;
		int num = 0;
		bool collision = false;
		int num1232;
		public override void AI()
		{
			if (Main.dayTime) {
				num1232++;
				if (num1232 >= Main.rand.Next(100, 700)) {
					NPC.active = false;
					NPC.netUpdate = true;
				}
			}
			num++;
			if (num >= Main.rand.Next(100, 400)) {
				num = 0;
			}
			if (!Main.dayTime) {
				Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.148f * 2, 0.191f * 2, .255f * 2);
			}
			NPC.spriteDirection = -NPC.direction;
			int npcXTile = (int)(NPC.Center.X / 16);
			int npcYTile = (int)(NPC.Center.Y / 16);
			for (int y = npcYTile; y > Math.Max(0, npcYTile - 100); y--) {
				if (Main.tile[npcXTile, y].LiquidAmount != 255) {
					int liquid = (int)Main.tile[npcXTile, y].LiquidAmount;
					float up = (liquid / 255f) * 16f;
					NPC.position.Y = (y + 1) * 16f - up;
					break;
				}
			}
			if (!collision) {
				NPC.velocity.X = .5f * Main.windSpeedCurrent;
			}
			else {
				NPC.velocity.X = -.5f * Main.windSpeedCurrent;
			}
			if (NPC.collideX || NPC.collideY) {
				NPC.velocity.X *= -1f;
				if (!collision) {
					collision = true;
				}
				else {
					collision = false;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			drawColor = new Color(148 - (int)(num / 3 * 4), 212 - (int)(num / 3 * 4), 255 - (int)(num / 3 * 4), 255 - num);
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 8), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
	}
}
