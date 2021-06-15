using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.StarplateGlove
{
	public class StarplateGloveProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fist of the north Starplate");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.hide = false;
			projectile.friendly = false;
			projectile.magic = true;
			projectile.timeLeft = 2;
			projectile.ignoreWater = true;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		bool returning = false;
		bool rightClick = true;
		Vector2 target = Vector2.Zero;
		int counter;
		public override void AI()
		{
			projectile.timeLeft = 2;
			counter++;
			Player player = Main.player[projectile.owner];
			if (player.HeldItem.type != ModContent.ItemType<StarplateGlove>())
			{
				returning = true;
			}
			Vector2 direction = Main.MouseWorld - projectile.Center;
			direction.Normalize();
			projectile.rotation = direction.ToRotation() + 1.57f;
			if (!returning)
			{
				if (target == Vector2.Zero)
				{
					target = Main.MouseWorld;
				}
				Vector2 vel = target - projectile.position;
				float speed = (float)Math.Sqrt(vel.Length()) / 2;
				vel.Normalize();
				vel *= speed;
				projectile.velocity = vel;
				if (!Main.mouseRight)
				{
					rightClick = false;
				}
				if (Main.mouseRight && !rightClick)
				{
					returning = true;
				}
				if (Main.mouseLeft && counter % 7 == 0)
				{
					if (player.statMana <= 0)
						return;
					player.statMana -= 6;
					player.manaRegenDelay = 60;
					Vector2 position = projectile.Center;
					float speedX = direction.X * 10;
					float speedY = direction.Y * 10;
				
					float stray = Main.rand.NextFloat(-0.7f, 0.7f);
					Vector2 speed2 = new Vector2(speedX,speedY).RotatedBy(stray);
					//speed *= Main.rand.NextFloat(0.9f, 1.1f);
					position += speed2 * 8;
					int type = Main.rand.Next(2)==0 ? mod.ProjectileType("StargloveChargeOrange") : mod.ProjectileType("StargloveChargePurple");
					int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, projectile.damage, projectile.knockBack, player.whoAmI);
					if (type == mod.ProjectileType("StargloveChargePurple"))
					{
						for (float num2 = 0.0f; (double)num2 < 10; ++num2) {
							int dustIndex = Dust.NewDust(position - speed2 * 3, 2, 2, 111, 0f, 0f, 0, default(Color), 1.5f);
							Main.dust[dustIndex].noGravity = true;
							Main.dust[dustIndex].velocity = Vector2.Normalize((speed2 * 5).RotatedBy(Main.rand.NextFloat(6.28f))) * 2.5f;
						}
						for (int j = 0; j < 5; j++)
							Projectile.NewProjectile(position, speed2, mod.ProjectileType("StargloveOrbiterPurple"), 0, 0, player.whoAmI, proj);
					}
					else
					{
						for (float num2 = 0.0f; (double)num2 < 10; ++num2) {
							int dustIndex = Dust.NewDust(position - speed2 * 3, 2, 2, 6, 0f, 0f, 0, default(Color), 2f);
							Main.dust[dustIndex].noGravity = true;
							Main.dust[dustIndex].velocity = Vector2.Normalize((speed2 * 8).RotatedBy(Main.rand.NextFloat(6.28f))) * 2.5f;
						}
						for (int j = 0; j < 5; j++)
							Projectile.NewProjectile(position, speed2, mod.ProjectileType("StargloveOrbiterOrange"), 0, 0, player.whoAmI, proj);
					}
				}
			}
			else
			{
				Vector2 vel = player.Center - projectile.position;
				if (vel.Length() < 40 || vel.Length() > 1500)
				{
					projectile.active = false;
				}
				vel.Normalize();
				vel *= 20;
				projectile.velocity = vel;
				projectile.rotation =vel.ToRotation() - 1.57f;
			}
		}
	}
}