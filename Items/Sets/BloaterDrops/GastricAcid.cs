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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gastric Acid");
		}

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
			projectile.extraUpdates = 10;
			projectile.timeLeft = 20;
			aiType = ProjectileID.FlamethrowerTrap;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(2))
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 138, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 0, default, Main.rand.NextFloat(0.6f, 0.9f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}
