using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class QuicksilverArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Arrow");
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
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverCoin);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int h = 0; h < 2; h++)
			{
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * (2 * MathHelper.Pi);
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				Projectile.NewProjectile(projectile.position, vel,
					mod.ProjectileType("QuicksilverBolt1"), 35, 1, projectile.owner);
			}
		}

	}
}
