using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class LittleBouncingSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Ball");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.height = 32;
			Projectile.width = 30;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 4;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			Projectile.Bounce(oldVelocity, 0.5f);

			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f * Projectile.velocity.X;
			Projectile.velocity.Y += 0.4f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 8; num621++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 100, default, .7f);
			}
		}
	}
}