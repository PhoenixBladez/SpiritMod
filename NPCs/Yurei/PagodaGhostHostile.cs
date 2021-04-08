using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Items.Consumable.Food;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Yurei
{
	public class PagodaGhostHostile : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Disturbed Yuurei");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			if (NPC.downedBoss2) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 25;
				npc.noGravity = true;
				npc.defense = 8;
				npc.lifeMax = 80;
			}
			if (NPC.downedBoss3) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 30;
				npc.noGravity = true;
				npc.defense = 11;
				npc.lifeMax = 130;
			}
			else {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 23;
				npc.noGravity = true;
				npc.defense = 4;
				npc.lifeMax = 50;
			}
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .1f;
			npc.noTileCollide = true;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.YureiBanner>();
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 66, 0f, -2f, 0, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Ramen>());
            }
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Sushi>());
            }
        }
        public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.alpha += 1;
			if (npc.alpha >= 180) {

				int angle = Main.rand.Next(360);
				int distX = (int)(Math.Sin(angle * (Math.PI / 180)) * 90);
				int distY = (int)(Math.Cos(angle * (Math.PI / 180)) * 160);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				npc.position.X = player.position.X + distX;
				npc.position.Y = player.position.Y + distY;
				Gore.NewGore(npc.position, npc.velocity, 99);
				npc.alpha = 0;
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
			}
			npc.spriteDirection = npc.direction;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			for (int k = 0; k < npc.oldPos.Length; k++) {
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}
	}
}
