﻿using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet.Crimbine
{
	public class CrimbineBlob : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Bloody Clump");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.height = 12;
			projectile.width = 3;
		}

		public override void AI()
		{
			projectile.rotation += .1f;
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 22) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<BloodCorrupt>(), 180);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int k = 0; k < 6; k++) 
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			return true;
		}

		public override void Kill(int timeLeft) => Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
	}
}
