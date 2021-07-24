using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloaterDrops
{
	public class GastricAcid : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gastric Acid");

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 20;
			aiType = ProjectileID.FlamethrowerTrap;
		}

		public override void AI()
		{
			//if (projectile.timeLeft < 12) //Useless when the projectile is invisible
			//	projectile.alpha = (int)((1 - projectile.timeLeft / 12f) * 255f); 

			if (projectile.timeLeft < 10)
				projectile.velocity *= 0.95f;

			if (!projectile.wet)
				projectile.velocity.Y += 0.2f;

			float scaleMult = 1f;
			if (projectile.timeLeft < 3)
				scaleMult = 1.75f; //Final dusts are bigger
			int dusts = 1 + (Main.rand.NextBool(2) ? 1 : 0);
			for (int i = 0; i < dusts; ++i)
			{
				float xVel = Main.rand.NextFloat(-1f, 1f) * scaleMult;
				float yVel = Main.rand.NextFloat(-1f, 1f) * scaleMult;
				Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<GastricDust>(), xVel, yVel, 0, default, Main.rand.NextFloat(0.8f, 1f) * scaleMult);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}
