using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowCircleRune : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Circle Rune");
		}

		public override void SetDefaults()
		{
			projectile.width = 86;
			projectile.height = 80;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 3600;
			projectile.sentry = true;
		}

		public override bool PreAI()
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Shadowflame, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

			projectile.rotation += 0.05f * projectile.direction;
			return true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 30)
			{
				projectile.frameCounter = 0;
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(projectile, false))
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
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
							num5 = num4 / num5;

						value *= num5;
						int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("CrystalShadow"), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

	}
}
