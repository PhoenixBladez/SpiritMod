using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class EoWSpearProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Scourge");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);

			AIType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(8)) {
				int n = Main.rand.Next(1, 3);
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation)) * 5.5f;
					Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.TinyEater, Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
				}
			}
		}
	}
}
