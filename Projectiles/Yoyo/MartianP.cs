using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MartianP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrestrial Ultimatum");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.TheEyeOfCthulhu);
			projectile.damage = 124;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.TheEyeOfCthulhu;
		}

		public override void PostAI()
		{
			projectile.rotation -= (10);
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 200)
			{
				projectile.frameCounter = 0;
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ProjectileID.Electrosphere, projectile.damage, projectile.owner, 0, 0f);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 7f;
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 226);
			}
		}

	}
}
