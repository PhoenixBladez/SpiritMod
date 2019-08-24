using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class FieryFlailProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Flail");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 22;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(projectile.whoAmI);

			for (int i = 0; i < 200; ++i)
			{
				if (Main.npc[i].active && !Main.npc[i].boss)
				{
					if (Main.npc[i].friendly == false)
					{
						if ((Main.npc[i].Center - projectile.Center).Length() < 128)
							Main.npc[i].AddBuff(BuffID.OnFire, 180);
					}
				}
			}
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 2f;
			Main.dust[dust].scale = 2f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/FieryFlail_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}
