using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
		}
		int sync;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && target.velocity != Vector2.Zero && target.knockBackResist != 0) {
				Main.npc[target.whoAmI].velocity.Y = 6f;
				sync = target.whoAmI;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 10; i++) 
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Dirt, (float)(Main.rand.Next(5) - 2), (float)(Main.rand.Next(5) - 2), 133);
		}
	}
}
