using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class PoisonArrowProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poisoned Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 9;
			projectile.height = 17;

			projectile.penetrate = 2;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;

			projectile.ranged = true;
			projectile.friendly = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 93);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.Poisoned, 180);
		}

	}
}
