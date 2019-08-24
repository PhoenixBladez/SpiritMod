using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class ReachBossSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reach Spike");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 20;
			projectile.height = 28;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowHostile;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 280;
		}

		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 20;
			projectile.height = 28;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num621 = 0; num621 < 20; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2, 0f, 0f, 100, default(Color), 1f);
			}
		}

	}
}
