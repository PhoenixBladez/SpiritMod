
using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class FlakeRocketProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flake Rocket");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.RocketI);
			projectile.width = 26;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.penetrate = 1;
			projectile.ranged = true;
			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			var offset = new Vector2(projectile.Center.X - 13, projectile.Center.Y).RotatedBy(projectile.rotation, projectile.Center) + new Vector2(-4, -4);
			int dust = Dust.NewDust(offset, 0, 0, 68, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(offset, 0, 0, 180, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0.6f;
			Main.dust[dust2].velocity *= 0.1f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = .8f;
			/*for(int i = 0; i < 4; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 180);
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}*/
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			=> target.AddBuff(ModContent.BuffType<CryoCrush>(), 300, true);
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			damage /= 2;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (target.townNPC) {
				return false;
			}
			return base.CanHitNPC(target);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CryoFire>(), (int)(projectile.damage * 0.75f + 0.5f), projectile.knockBack, projectile.owner);

			projectile.position.X = projectile.position.X + (projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (projectile.height / 2);
			projectile.width = 30;
			projectile.height = 30;
			projectile.position.X = projectile.position.X - (projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (projectile.height / 2);

			for (int num623 = 0; num623 < 40; num623++) {
				int num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					68, 0f, 0f, 100, default, 1f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					68, 0f, 0f, 100, default, 1f);
				Main.dust[num624].velocity *= 2f;
			}
		}
	}
}
