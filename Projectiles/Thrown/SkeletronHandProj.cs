using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class SkeletronHandProj : ModProjectile
	{
		int timer = 0;
		// USE THIS DUST: 261
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Cutter");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 22;

			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;

			projectile.penetrate = 3;

			projectile.timeLeft = 160;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("SkeletronHand"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 37);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			timer++;
			if (timer == 20 || timer == 40 || timer == 80)
				projectile.velocity *= 0.1f;
			else if (timer == 30 || timer == 90)
				projectile.velocity *= 10;
			else if (timer >= 100)
				timer = 0;

			return false;
		}

	}
}
