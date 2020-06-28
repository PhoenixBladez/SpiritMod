using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class MarbleBidentProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bident");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);
			aiType = ProjectileID.Trident;
		}
		public override void AI()
		{
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 2;

			Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			{
				if(Main.rand.Next(4) == 0 && !target.SpawnedFromStatue) {
					target.AddBuff(BuffID.Midas, 180);
				}
				if(!target.SpawnedFromStatue && Main.rand.Next(4) == 0 && target.HasBuff(BuffID.Midas)) {
					int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
					0, 0, ModContent.ProjectileType<GildedFountain>(), projectile.damage, projectile.knockBack, projectile.owner);
				}
			}
		}
	}
}
