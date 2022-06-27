
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class OrionBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		int timer = 1;
		public override void AI()
		{
			timer--;
			for (int i = 0; i < 6; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.Flare_Blue);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
			if (timer == 0) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<StarTrail>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                Main.projectile[p].DamageType = DamageClass.Ranged;
                timer = 30;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.position);
	}
}
