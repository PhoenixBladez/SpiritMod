using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class GigazapperProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gigazapper");
		}

		int timer = 0;

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.JavelinFriendly);
			aiType = ProjectileID.JavelinFriendly;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("ElectrifiedV2"), 240, true);
		}

		public override void AI()
		{
			if (Main.rand.Next(4) == 0)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 226);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 226);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
