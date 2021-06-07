using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticWizardBallEnergyEffect : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Arcane Energy");

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.penetrate = 2;
            projectile.hide = true;
			projectile.timeLeft = 300;
		}

		public override void AI()
		{
			int num1 = ModContent.ProjectileType<MysticWizardBall>();
			float num2 = 60f;
			float x = 0.3f;
			float y = 0.3f;
			bool flag2 = false;
			if ((double)projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)projectile.ai[1];
				if (Main.projectile[index1].active && Main.projectile[index1].type == num1) {
					if (!flag2 && Main.projectile[index1].oldPos[1] != Vector2.Zero)
						projectile.position = projectile.position + Main.projectile[index1].position - Main.projectile[index1].oldPos[1];
				}
				else {
					projectile.ai[0] = num2;
					flag4 = false;
                    projectile.Kill();
				}
				if (flag4 && !flag2) {
                    projectile.velocity += new Vector2((float)Math.Sign(Main.projectile[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.projectile[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                    if (projectile.velocity.Length() > 7f)
                    {
                        projectile.velocity *= 7f / projectile.velocity.Length();
                    }
                }
			}
			for (int i = 0; i < 5; i++) {
				if (projectile.width == 8) {
					float x1 = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y1 = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x1, y1), 2, 2, 180);
					Main.dust[num].velocity = projectile.velocity;
					Main.dust[num].noGravity = true;
                    Main.dust[num].scale = projectile.scale;
                }
			}

		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.Frostburn, 120);
	}
}
