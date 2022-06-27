using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class ShadowBall : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penumbral Sphere");
			Main.npcFrameCount[NPC.type] = 4;

		}

		public override void SetDefaults()
		{
			NPC.width = NPC.height = 40;
			NPC.alpha = 255;

			NPC.lifeMax = 1;
			NPC.damage = 50;
			NPC.friendly = false;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		public override bool PreAI()
		{
			if (NPC.ai[1] == 0f) {
				NPC.ai[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 34);
			}
			else if (NPC.ai[1] == 1f && Main.netMode != NetmodeID.MultiplayerClient) {
				int target = -1;
				float distance = 2000f;
				for (int k = 0; k < 255; k++) {
					if (Main.player[k].active && !Main.player[k].dead) {
						Vector2 center = Main.player[k].Center;
						float currentDistance = Vector2.Distance(center, NPC.Center);
						if ((currentDistance < distance || target == -1) && Collision.CanHit(NPC.Center, 1, 1, center, 1, 1)) {
							distance = currentDistance;
							target = k;
						}
					}
				}
				if (target != -1) {
					NPC.ai[1] = 21f;
					NPC.ai[0] = target;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[1] > 20f && NPC.ai[1] < 200f) {
				NPC.ai[1] += 1f;
				int target = (int)NPC.ai[0];
				if (!Main.player[target].active || Main.player[target].dead) {
					NPC.ai[1] = 1f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
				}
				else {
					float num6 = NPC.velocity.ToRotation();
					Vector2 vector2 = Main.player[target].Center - NPC.Center;

					float targetAngle = vector2.ToRotation();
					if (vector2 == Vector2.Zero) {
						targetAngle = num6;
					}
					float num7 = num6.AngleLerp(targetAngle, 0.008f);
					NPC.velocity = new Vector2(NPC.velocity.Length(), 0f).RotatedBy((double)num7, default);
				}
			}

			if (NPC.ai[1] >= 1f && NPC.ai[1] < 20f) {
				NPC.ai[1] += 1f;
				if (NPC.ai[1] == 20f)
					NPC.ai[1] = 1f;
			}

			if (NPC.alpha <= 0)
				NPC.alpha = 0;
			else
				NPC.alpha -= 40;

			NPC.spriteDirection = NPC.direction;

			NPC.localAI[0] += 1f;
			if (NPC.localAI[0] == 12f) {
				NPC.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (NPC.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(NPC.Center, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = NPC.Center + vector2;
					Main.dust[num8].velocity = NPC.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			Vector2 position = NPC.Center + Vector2.Normalize(NPC.velocity) * 10;
			Dust newDust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f)];
			newDust.position = position;
			newDust.velocity = NPC.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + NPC.velocity / 4;
			newDust.position += NPC.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1)];
			newDust.position = position;
			newDust.velocity = NPC.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + NPC.velocity / 4;
			newDust.position += NPC.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;

			for (int index1 = 0; index1 < 6; ++index1) {
				float x = (NPC.Center.X - 2);
				float xnum2 = (NPC.Center.X + 2);
				float y = (NPC.Center.Y);
				if (NPC.direction == -1) {
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.ShadowbeamStaff, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
				else if (NPC.direction == 1) {
					int index2 = Dust.NewDust(new Vector2(xnum2, y), 1, 1, DustID.ShadowbeamStaff, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = xnum2;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
			Lighting.AddLight(NPC.Center, 1.1f, 0.3f, 1.1f);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			bool expertMode = Main.expertMode;
			int dam = expertMode ? 19 : 35;
			int p = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<Projectiles.HostileWrath>(), dam, 1, Main.myPlayer, 0, 0);
			Main.projectile[p].timeLeft = 30;

			SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 14);
			if (NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool CheckDead()
		{
			for (int num383 = 0; num383 < 5; num383++) {
				int num384 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Shadowflame, 0f, 0f, 0, default, 1f);
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
