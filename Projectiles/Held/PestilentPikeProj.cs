using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class PestilentPikeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Pike");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);

			aiType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<BlightedFlames>(), 260, false);

			MyPlayer mp = Main.player[projectile.owner].GetSpiritPlayer();
			mp.PutridHits++;
			if (mp.putridSet && mp.PutridHits >= 4)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, ModContent.ProjectileType<CursedFlame>(), projectile.damage * 3 / 2, 0f, projectile.owner, 0f, 0f);
				mp.PutridHits = 0;
			}
		}

	}
}
