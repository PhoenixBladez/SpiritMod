
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class Scapula : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soaring Scapula");
		}
		float ai0 = 0;
		float ai1 = 0;
		float localai0 = 0;
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && target.velocity != Vector2.Zero && target.knockBackResist != 0) {
				target.velocity.Y = 6f;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 0, (float)(Main.rand.Next(5) - 2), (float)(Main.rand.Next(5) - 2), 133);
			}
		}
	}
}
