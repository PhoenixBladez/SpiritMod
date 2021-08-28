using Microsoft.Xna.Framework;
using Terraria;
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
			projectile.hostile = true;
			projectile.height = 32;
			projectile.width = 30;
			projectile.friendly = false;
			projectile.aiStyle = -1;
			projectile.penetrate = 4;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			projectile.Bounce(oldVelocity, 0.5f);

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f * projectile.velocity.X;
			projectile.velocity.Y += 0.4f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 8; num621++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Grass, 0f, 0f, 100, default, .7f);
			}
		}
	}
}