using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ThornSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Spike");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 32;
			projectile.height = 32;
			projectile.timeLeft = 300;
			projectile.penetrate = 4;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			for(int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(projectile.position, projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			projectile.scale -= .06f;
			projectile.penetrate--;
			if(projectile.penetrate <= 0)
				projectile.Kill();
			else {
				aiType = ProjectileID.Shuriken;
				if(projectile.velocity.X != oldVelocity.X) {
					projectile.velocity.X = -oldVelocity.X;
				}
				if(projectile.velocity.Y != oldVelocity.Y) {
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.velocity *= 0.75f;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			for(int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(projectile.position, projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			{
				target.AddBuff(BuffID.Poisoned, 240, true);

			}
			projectile.penetrate--;
			if(projectile.penetrate <= 0)
				projectile.Kill();
			else {
				aiType = ProjectileID.Shuriken;

				projectile.velocity.X *= -1;
				projectile.velocity *= 0.75f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(projectile.position, projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}