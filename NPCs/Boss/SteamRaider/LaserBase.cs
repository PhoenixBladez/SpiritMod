using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class LaserBase : ModNPC
	{
		Vector2 direction9 = Vector2.Zero;
		//private bool shooting;
		private int timer = 0;
		//private bool inblock = true;
		Vector2 target = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Launcher");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 46;
			npc.damage = 0;
			npc.defense = 12;
			npc.noTileCollide = true;
			npc.dontTakeDamage = true;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit4;
			npc.value = 160f;
			npc.knockBackResist = .16f;
			npc.noGravity = true;
			npc.dontCountMe = true;
		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/LaserBase_Glow"));
			}
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];

			float num5 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
			if (!(npc.ai[0] >= 100 && npc.ai[0] <= 130)) {
				if (num7 < 0f) {
					num7 += 6.283f;
				}
				else if ((double)num7 > 6.283) {
					num7 -= 6.283f;
				}
			}
			npc.spriteDirection = npc.direction;
			if(npc.ai[0] == 0) {
				npc.ai[1] = Main.rand.Next(160, 190);
				npc.netUpdate = true;
			}
			npc.ai[0]++;
			if (npc.ai[0] >= npc.ai[1]) {
				Main.PlaySound(SoundID.Item, npc.Center, 110);
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
					}
				}
				if (Main.expertMode)
					npc.Transform(ModContent.NPCType<SuicideLaser>());
				else
					npc.active = false;
				npc.netUpdate = true;
			}
			else {
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
			}
			if (npc.ai[0] <= 75) {
				direction9 = player.Center - npc.Center;
				direction9.Normalize();
			}
			if (npc.ai[0] >= 60 && npc.ai[0] <= 110 & npc.ai[0] % 2 == 0) {
				{
					int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 226);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].scale *= .8f;
					Main.dust[dust].noGravity = true;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-80, 81), (float)Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = npc.Center - vector2_3;
				}
			}
			if (npc.alpha != 255) {
				if (Main.rand.NextFloat() < 0.5f) {
					Dust dust;
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X - 10, npc.Center.Y);
					dust = Terraria.Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
				}
				if (Main.rand.NextFloat() < 0.5f) {
					Dust dust;
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X + 10, npc.Center.Y);
					dust = Terraria.Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
				}

					if (npc.ai[0] == 110) //change to frame related later
					{
						Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 53);
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 40, (float)direction9.Y * 40, ModContent.ProjectileType<StarLaser>(), NPCUtils.ToActualDamage(55, 1.5f), 1, Main.myPlayer);
					}
					if (npc.ai[0] < 110 && npc.ai[0] > 75 && npc.ai[0] % 3 == 0) {
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, ModContent.ProjectileType<StarLaserTrace>(), NPCUtils.ToActualDamage(27, 1.5f), 1, Main.myPlayer);
					}
				npc.rotation = direction9.ToRotation() - 1.57f;
			}
			return false;
		}
	}
}
