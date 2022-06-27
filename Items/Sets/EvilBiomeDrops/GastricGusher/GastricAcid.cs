using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.EvilBiomeDrops.GastricGusher
{
	public class GastricAcid : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gastric Acid");

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 20;
			AIType = ProjectileID.FlamethrowerTrap;
		}

		public override void AI()
		{
			//if (projectile.timeLeft < 12) //Useless when the projectile is invisible
			//	projectile.alpha = (int)((1 - projectile.timeLeft / 12f) * 255f); 

			if (Projectile.timeLeft < 10)
				Projectile.velocity *= 0.95f;

			if (!Projectile.wet)
				Projectile.velocity.Y += 0.2f;

			float scaleMult = 1f;
			if (Projectile.timeLeft < 3)
				scaleMult = 1.75f; //Final dusts are bigger
			int dusts = 1 + (Main.rand.NextBool(2) ? 1 : 0);
			for (int i = 0; i < dusts; ++i)
			{
				float xVel = Main.rand.NextFloat(-1f, 1f) * scaleMult;
				float yVel = Main.rand.NextFloat(-1f, 1f) * scaleMult;
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<GastricDust>(), xVel, yVel, 0, default, Main.rand.NextFloat(0.8f, 1f) * scaleMult);
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
