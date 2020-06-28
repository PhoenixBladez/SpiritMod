using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class GlowStingSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph Blade");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);

			aiType = ProjectileID.Trident;
		}
		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
		}
	}
}