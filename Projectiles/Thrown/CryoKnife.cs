using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class CryoKnife : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bomb");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 14;

			projectile.aiStyle = 2;

			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 69);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<CryoExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);
			for (int i = 0; i < 5; i++) {
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 180);
				Main.dust[d].scale = .5f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 240);
		}
		public override bool PreAI()
		{
			//    projectile.rotation += 0.1f;
			return true;
		}
	}
}