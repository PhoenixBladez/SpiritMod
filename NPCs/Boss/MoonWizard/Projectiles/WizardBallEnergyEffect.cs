using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class WizardBallEnergyEffect : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Arcane Energy");

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
            Projectile.hide = true;
			Projectile.timeLeft = 300;
		}

		public override void AI()
		{
			int num1 = ModContent.ProjectileType<WizardBall>();
			float num2 = 60f;
			float x = 0.3f;
			float y = 0.3f;
			bool flag2 = false;
			if ((double)Projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)Projectile.ai[1];
				if (Main.projectile[index1].active && Main.projectile[index1].type == num1) {
					if (!flag2 && Main.projectile[index1].oldPos[1] != Vector2.Zero)
						Projectile.position = Projectile.position + Main.projectile[index1].position - Main.projectile[index1].oldPos[1];
				}
				else {
					Projectile.ai[0] = num2;
					flag4 = false;
                    Projectile.Kill();
				}
				if (flag4 && !flag2) {
                    Projectile.velocity += new Vector2((float)Math.Sign(Main.projectile[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.projectile[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
                    if (Projectile.velocity.Length() > 7f)
                    {
                        Projectile.velocity *= 7f / Projectile.velocity.Length();
                    }
                }
			}
			for (int i = 0; i < 5; i++) {
				if (Projectile.width == 8) {
					float x1 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float y1 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x1, y1), 2, 2, DustID.DungeonSpirit);
					Main.dust[num].velocity = Projectile.velocity;
					Main.dust[num].noGravity = true;
                    Main.dust[num].scale = Projectile.scale;
                }
			}

		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.Frostburn, 120);
	}
}
