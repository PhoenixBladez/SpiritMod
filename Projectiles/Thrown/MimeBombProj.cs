using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;
using System;

namespace SpiritMod.Projectiles.Thrown
{
	public class MimeBombProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mime Bomb");
		}

		public override void SetDefaults()
		{
			projectile.width = 17;
			projectile.height = 17;

			projectile.aiStyle = 2;
			projectile.timeLeft = 180;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
			projectile.penetrate = 494;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (target.townNPC) {
				return false;
			}
			return base.CanHitNPC(target);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!projectile.hostile) {
				projectile.Kill();
			}
		}
		public override bool PreAI()
		{
			projectile.velocity.X *= 1.015f;
			return base.PreAI();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) {
				projectile.velocity.X = oldVelocity.X * -0.45f;
			}
			if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
				projectile.velocity.Y = oldVelocity.Y * -0.45f;
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			int sizeX = 120;
			int sizeY = 45;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = sizeX;
			projectile.height = sizeY;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			projectile.Damage();
			Main.projectileIdentity[projectile.owner, projectile.identity] = -1;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = (int)((float)sizeX / 5.8f);
			projectile.height = (int)((float)sizeY / 5.8f);
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			DustHelper.DrawDustImage(projectile.position, ModContent.DustType<MarbleDust>(), 0.075f, "SpiritMod/Effects/DustImages/Boom", 1.66f);
		}
	}
}
