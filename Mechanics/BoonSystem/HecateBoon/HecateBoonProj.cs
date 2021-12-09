using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoonProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Hecate");
			Main.projFrames[projectile.type] = 6;
		}


		public override void SetDefaults()
		{
			projectile.width = projectile.height = 18;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.frame = Main.rand.Next(6);
			projectile.timeLeft = 150;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
