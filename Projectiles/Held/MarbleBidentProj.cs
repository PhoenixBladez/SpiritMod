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
			Projectile.CloneDefaults(ProjectileID.Trident);
			AIType = ProjectileID.Trident;
		}
		public override void AI()
		{
			Vector2 position = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 2;

			Dust newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 0, default, 1f)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 0, default, 1)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4) && !target.SpawnedFromStatue)
				target.AddBuff(BuffID.Midas, 180);
			if (!target.SpawnedFromStatue && Main.rand.NextBool(4) && target.HasBuff(BuffID.Midas))
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<GildedFountain>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}
}