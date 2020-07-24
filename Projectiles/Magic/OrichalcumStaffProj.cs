using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class OrichalcumStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Petal");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.alpha = 255;
			projectile.timeLeft = 20;
			projectile.tileCollide = false;

		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i) {
				Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 4;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<OrichHoming>(), projectile.damage, projectile.knockBack, Main.myPlayer);
			}
		}

	}
}
