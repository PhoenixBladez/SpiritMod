using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class GraniteShard1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Shard");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 11;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.CrystalShard;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}
		public override void AI()
		{

			for (int index1 = 0; index1 < 5; ++index1) {
				float num1 = Projectile.velocity.X * 0.2f * (float)index1;
				float num2 = Projectile.velocity.Y * -0.200000002980232f * index1;
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].scale = .25f;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++) {
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
				Main.dust[index2].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
		}
	}
}