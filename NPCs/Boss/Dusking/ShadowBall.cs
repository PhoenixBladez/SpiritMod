using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class ShadowBall : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penumbral Sphere");
			Main.npcFrameCount[npc.type] = 4;

		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 40;
			npc.alpha = 255;

			npc.lifeMax = 1;
			npc.damage = 50;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool PreAI()
		{
			if(npc.ai[1] == 0f) {
				npc.ai[1] = 1f;
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 34);
			} else if(npc.ai[1] == 1f && Main.netMode != 1) {
				int target = -1;
				float distance = 2000f;
				for(int k = 0; k < 255; k++) {
					if(Main.player[k].active && !Main.player[k].dead) {
						Vector2 center = Main.player[k].Center;
						float currentDistance = Vector2.Distance(center, npc.Center);
						if((currentDistance < distance || target == -1) && Collision.CanHit(npc.Center, 1, 1, center, 1, 1)) {
							distance = currentDistance;
							target = k;
						}
					}
				}
				if(target != -1) {
					npc.ai[1] = 21f;
					npc.ai[0] = target;
					npc.netUpdate = true;
				}
			} else if(npc.ai[1] > 20f && npc.ai[1] < 200f) {
				npc.ai[1] += 1f;
				int target = (int)npc.ai[0];
				if(!Main.player[target].active || Main.player[target].dead) {
					npc.ai[1] = 1f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				} else {
					float num6 = npc.velocity.ToRotation();
					Vector2 vector2 = Main.player[target].Center - npc.Center;

					float targetAngle = vector2.ToRotation();
					if(vector2 == Vector2.Zero) {
						targetAngle = num6;
					}
					float num7 = num6.AngleLerp(targetAngle, 0.008f);
					npc.velocity = new Vector2(npc.velocity.Length(), 0f).RotatedBy((double)num7, default(Vector2));
				}
			}

			if(npc.ai[1] >= 1f && npc.ai[1] < 20f) {
				npc.ai[1] += 1f;
				if(npc.ai[1] == 20f)
					npc.ai[1] = 1f;
			}

			if(npc.alpha <= 0)
				npc.alpha = 0;
			else
				npc.alpha -= 40;

			npc.spriteDirection = npc.direction;

			npc.localAI[0] += 1f;
			if(npc.localAI[0] == 12f) {
				npc.localAI[0] = 0f;
				for(int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(npc.Center, 0, 0, 173, 0f, 0f, 160, default(Color), 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = npc.Center + vector2;
					Main.dust[num8].velocity = npc.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			Vector2 position = npc.Center + Vector2.Normalize(npc.velocity) * 10;
			Dust newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 173, 0f, 0f, 0, default(Color), 1f)];
			newDust.position = position;
			newDust.velocity = npc.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
			newDust.position += npc.velocity.RotatedBy(Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 173, 0f, 0f, 0, default(Color), 1)];
			newDust.position = position;
			newDust.velocity = npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
			newDust.position += npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;

			for(int index1 = 0; index1 < 6; ++index1) {
				float x = (npc.Center.X - 2);
				float xnum2 = (npc.Center.X + 2);
				float y = (npc.Center.Y);
				if(npc.direction == -1) {
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 173, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				} else if(npc.direction == 1) {
					int index2 = Dust.NewDust(new Vector2(xnum2, y), 1, 1, 173, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = xnum2;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
			Lighting.AddLight(npc.Center, 1.1f, 0.3f, 1.1f);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			bool expertMode = Main.expertMode;
			int dam = expertMode ? 19 : 35;
			int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<HostileWrath>(), dam, 1, Main.myPlayer, 0, 0);
			Main.projectile[p].timeLeft = 30;

			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
			if(npc.life <= 0) {
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for(int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if(Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for(int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool CheckDead()
		{
			for(int num383 = 0; num383 < 5; num383++) {
				int num384 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num384].noGravity = true;
				Main.dust[num384].velocity *= 1.5f;
				Main.dust[num384].scale *= 0.9f;
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);
		}
	}
}
