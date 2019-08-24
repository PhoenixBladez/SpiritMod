using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MysticProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Amarok);
			projectile.damage = 54;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Amarok;
		}

		int manaUsageTimer;
		public override bool PreAI()
		{
			if (manaUsageTimer++ % 10 == 0) // Uses mana every 10 frames, so 6 times a second.
			{
				Player player = Main.player[projectile.owner];
				if (!player.CheckMana(player.inventory[player.selectedItem].mana, true, false))
					projectile.Kill(); // Or whatever you do to 'end' your projectile.
			}
			return true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 28)
			{
				projectile.frameCounter = 0;
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("MysticBall"), projectile.damage, projectile.owner, 0, 0f);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 12f;
			}
		}

	}
}
