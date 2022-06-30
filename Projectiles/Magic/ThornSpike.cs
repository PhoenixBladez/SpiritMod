using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 4;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(Projectile.GetSource_Misc("TileHit"), Projectile.position, Projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			Projectile.scale -= .06f;
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else {
				AIType = ProjectileID.Shuriken;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(Projectile.GetSource_OnHit(target), Projectile.position, Projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			{
				target.AddBuff(BuffID.Poisoned, 240, true);

			}
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else {
				AIType = ProjectileID.Shuriken;

				Projectile.velocity.X *= -1;
				Projectile.velocity *= 0.75f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, 911, goreScale);
				Main.gore[a].timeLeft = 15;
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}