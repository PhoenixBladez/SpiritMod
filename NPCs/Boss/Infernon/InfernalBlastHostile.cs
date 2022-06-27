
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	public class InfernalBlastHostile : ModProjectile
	{
		int target;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Dust");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 12;

			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;

			Projectile.timeLeft = 120;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 16; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

			if (Projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				target = -1;
				float distance = 2000f;
				for (int k = 0; k < 255; k++) {
					if (Main.player[k].active && !Main.player[k].dead) {
						Vector2 center = Main.player[k].Center;
						float currentDistance = Vector2.Distance(center, Projectile.Center);
						if (currentDistance < distance || target == -1) {
							distance = currentDistance;
							target = k;
						}
					}
				}
				if (target != -1) {
					Projectile.ai[0] = 1;
					Projectile.netUpdate = true;
				}
			}
			else if (target >= 0 && target < Main.maxPlayers) {
				Player targetPlayer = Main.player[target];
				if (!targetPlayer.active || targetPlayer.dead) {
					target = -1;
					Projectile.ai[0] = 0;
					Projectile.netUpdate = true;
				}
				else {
					float currentRot = Projectile.velocity.ToRotation();
					Vector2 direction = targetPlayer.Center - Projectile.Center;
					float targetAngle = direction.ToRotation();
					if (direction == Vector2.Zero) {
						targetAngle = currentRot;
					}

					float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
					Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0f).RotatedBy(desiredRot, default);
				}
			}
			return true;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 8) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > 5)
					Projectile.frame = 0;
			}

			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 8) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 4; i < 31; i++) {
				float x = Projectile.oldVelocity.X * (30f / i);
				float y = Projectile.oldVelocity.Y * (30f / i);
				int newDust = Dust.NewDust(new Vector2(Projectile.oldPosition.X - x, Projectile.oldPosition.Y - y), 8, 8, DustID.Torch, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 0.5f;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.OnFire, 180, true);

			Projectile.Kill();
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			writer.Write(target);
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			target = reader.Read();
		}

	}
}
