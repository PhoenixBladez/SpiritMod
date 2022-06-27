using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class BrambleTrap : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Trap");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.damage = 1;
			Projectile.penetrate = -1;
		//	projectile.alpha = 255;
			Projectile.timeLeft = 5;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
		}

		bool activated = false;
		Vector2 groundpos = Vector2.Zero;
		bool stuck = false;
		public override bool PreAI()
		{
			if (Projectile.timeLeft == 1 && !stuck)
			{
				Projectile.position += Projectile.velocity * 8;
				Projectile.velocity = Vector2.Zero;
				stuck = true;
				Projectile.timeLeft = 580;
			}
			if (!activated && Projectile.timeLeft > 60)
			{
				activated = true;
				groundpos = Projectile.Center + new Vector2(0, 40);
				Projectile.friendly = true;
			}
			Projectile.velocity.X = 0;
			if (!activated) {
				Projectile.velocity.Y = 30;
				Projectile.timeLeft += 2;
			}
			else if (!stuck)
			{
				Vector2 mouse = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 dir9 = mouse - Projectile.Center;
				dir9.Normalize();
				dir9 *= 20;
				Projectile.velocity = dir9;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!stuck)
			{
				Projectile.timeLeft = (int)(580 * target.knockBackResist);
			}
			target.AddBuff(ModContent.BuffType<Stopped>(), (int)(240 * target.knockBackResist * 1.5f));
			Projectile.position += Projectile.velocity * 8;
			Projectile.velocity = Vector2.Zero;
			stuck = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != Projectile.velocity.Y && !activated) {
				activated = true;
				groundpos = Projectile.Center + new Vector2(0, 40);
				Projectile.friendly = true;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (activated)
			{
				ProjectileExtras.DrawChain(Projectile.whoAmI, groundpos - new Vector2(30, 0),
				"SpiritMod/Projectiles/BrambleTrap_Chain");
				ProjectileExtras.DrawChain(Projectile.whoAmI, groundpos + new Vector2(30, 0),
				"SpiritMod/Projectiles/BrambleTrap_Chain");
			}
			return false;
		}

	}
}