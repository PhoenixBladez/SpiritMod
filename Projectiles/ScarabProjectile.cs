
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ScarabProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Dust");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 3;
			projectile.alpha = 255;
			projectile.timeLeft = 40;
		}

		public override bool PreAI()
		{
			if (projectile.timeLeft > 20)
			{
				int newDust = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 8, 8, 85, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), Main.rand.NextFloat(.25f, .8f));
				newDust = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 8, 8, 36, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), Main.rand.NextFloat(.25f, .8f));
				newDust = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 8, 8, 32, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), Main.rand.NextFloat(.25f, .8f));
			}

			return false;
		}
	}
}
