using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow.Artifact
{
	public class StarPin1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Pin");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.penetrate = 4;
			projectile.timeLeft = 240;
			projectile.ranged = true;
			projectile.aiStyle = 1;
			projectile.CloneDefaults(ProjectileID.Bullet);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 244);
			}

			if (Main.rand.Next(2) == 1)
			{
				Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

				Projectile.NewProjectile(projectile.position, new Vector2(0, -5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);

				Projectile.NewProjectile(projectile.position, new Vector2(6, -2), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position, new Vector2(-6, -2), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);

				Projectile.NewProjectile(projectile.position, new Vector2(3, 5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position, new Vector2(-3, 5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
			}
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.MoonSongBlossom)
			{
				int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173);
				Main.dust[dust1].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust1].velocity = Vector2.Zero;
				Main.dust[dust2].velocity = Vector2.Zero;
				Main.dust[dust2].scale = 0.6f;
				Main.dust[dust1].scale = 0.6f;
				Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);
			}
			
			int dust3 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
			int dust4 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
			Main.dust[dust3].noGravity = true;
			Main.dust[dust4].noGravity = true;
			Main.dust[dust3].velocity = Vector2.Zero;
			Main.dust[dust4].velocity = Vector2.Zero;
			Main.dust[dust4].scale = 0.6f;
			Main.dust[dust3].scale = 0.6f;
			Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.OnFire, 180);

			if (crit)
			{
				Player player = Main.player[projectile.owner];
				for (int i = 0; i < 3; ++i)
				{
					if (Main.myPlayer == player.whoAmI)
					{
						Vector2 mouse = Main.MouseWorld;
						Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), projectile.position.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(10, 20), mod.ProjectileType("AstralArrow"), projectile.damage / 5 * 4, projectile.knockBack, Main.myPlayer);
					}
				}
			}

			if (Main.rand.Next(4) == 1)
			{
				Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

				Projectile.NewProjectile(projectile.position, new Vector2(0, -5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);

				Projectile.NewProjectile(projectile.position, new Vector2(6, -2), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position, new Vector2(-6, -2), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);

				Projectile.NewProjectile(projectile.position, new Vector2(3, 5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position, new Vector2(-3, 5), mod.ProjectileType("StarEnergyBolt"), projectile.damage, 0.4f, Main.myPlayer);
			}
		}

	}
}
