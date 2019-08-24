using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class NecropolisTrident : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necropolis Trident");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;

			projectile.aiStyle = 27;

			projectile.magic = true;
			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		public override void AI()
		{
			if (Main.rand.Next(3) == 0)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("BlightedFlames"), 260, false);

			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			mp.PutridHits++;
			if (mp.putridSet && mp.PutridHits >= 4)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("CursedFlame"), projectile.damage, 0f, projectile.owner, 0f, 0f);
				mp.PutridHits = 0;
			}
		}

	}
}
