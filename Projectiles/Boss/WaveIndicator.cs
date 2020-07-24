using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Boss;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class WaveIndicator : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Fragment");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.penetrate = 2;
			projectile.timeLeft = 280;
		}

		public override void AI()
		{
			int num1 = ModContent.NPCType<AncientFlyer>();
			float num2 = 210f;
			float x = 0.08f;
			float y = 0.1f;
			int Damage = 0;
			float num3 = 0.0f;
			bool flag1 = true;
			bool flag2 = false;
			bool flag3 = false;
			if ((double)projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)projectile.ai[1];
				if (Main.npc[index1].active && Main.npc[index1].type == num1) {
					if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
						projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
				}
				else {
					projectile.ai[0] = num2;
					flag4 = false;
				}
				if (flag4 && !flag2) {
					projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
				}
			}
			for (int i = 0; i < 5; i++) {
				if (projectile.width == 8) {
					float x1 = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y1 = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x1, y1), 2, 2, 172);
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}
			}

		}
		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(2) == 0) {
				int d = 172;
				for (int k = 0; k < 6; k++) {
					Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
					Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
				}

				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
				projectile.velocity *= 0f;
				projectile.width = 40;
				projectile.knockBack = 0;
			}
		}
	}
}
