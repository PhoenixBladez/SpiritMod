using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class Ghast : ModProjectile
	{
		private int lastFrame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast");
		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(575);
			base.projectile.damage = 25;
			this.aiType = 575;
			base.projectile.timeLeft = 7200;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.extraUpdates = 2;
			projectile.hide = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		int timer = 0;
		public override bool PreAI()
		{
			MyPlayer mp = Main.player[base.projectile.owner].GetModPlayer<MyPlayer>(base.mod);
			if (mp.player.dead)
				mp.Ghast = false;

			if (mp.Ghast)
				projectile.timeLeft = 2;

			for (int index1 = 0; index1 < 5; ++index1)
			{
				float num10 = projectile.velocity.X * 0.2f * (float)index1;
				float num20 = (float)-((double)projectile.velocity.Y * 0.200000002980232) * (float)index1;
				int index20 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 175, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index20].noGravity = true;
				Main.dust[index20].velocity *= 0.0f;
				Main.dust[index20].scale *= 0.7f;
				Main.dust[index20].position.X -= num10;
				Main.dust[index20].position.Y -= num20;
			}

			if (Main.rand.Next(50) == 0)
			{
				float num = 12000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1200f && Main.npc[i].CanBeChasedBy(projectile, false))
					{
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1)
				{
					bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag)
					{
						Vector2 value = Main.npc[num2].Center - projectile.Center;
						float num4 = 4f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
						{
							num5 = num4 / num5;
						}
						value *= num5;
						int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("WhiteSoul"), projectile.damage / 7 * 6, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
			{
				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 175, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != projectile.Center)
					{
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			});
		}
	}
}
