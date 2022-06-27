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
			Projectile.width = 17;
			Projectile.height = 17;

			Projectile.aiStyle = 2;
			Projectile.timeLeft = 180;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0;
			Projectile.penetrate = 494;
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
			if (!Projectile.hostile) {
				Projectile.Kill();
			}
		}
		public override bool PreAI()
		{
			Projectile.velocity.X *= 1.015f;
			return base.PreAI();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) {
				Projectile.velocity.X = oldVelocity.X * -0.45f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
				Projectile.velocity.Y = oldVelocity.Y * -0.45f;
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			int sizeX = 120;
			int sizeY = 45;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = sizeX;
			Projectile.height = sizeY;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			Projectile.Damage();
			Main.projectileIdentity[Projectile.owner, Projectile.identity] = -1;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = (int)((float)sizeX / 5.8f);
			Projectile.height = (int)((float)sizeY / 5.8f);
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

			DustHelper.DrawDustImage(Projectile.position, ModContent.DustType<MarbleDust>(), 0.075f, "SpiritMod/Effects/DustImages/Boom", 1.66f);
		}
	}
}
