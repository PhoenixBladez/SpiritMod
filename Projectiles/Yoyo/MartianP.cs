using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MartianP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrestrial Ultimatum");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.TheEyeOfCthulhu);
			Projectile.damage = 124;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.TheEyeOfCthulhu;
		}

		public override void PostAI()
		{
			Projectile.rotation -= (10);
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 200) {
				Projectile.frameCounter = 0;
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileID.Electrosphere, Projectile.damage, Projectile.owner, 0, 0f);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 7f;
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
			}
		}

	}
}
