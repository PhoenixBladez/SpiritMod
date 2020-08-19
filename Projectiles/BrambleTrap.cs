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
			projectile.hostile = false;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.damage = 1;
			projectile.penetrate = -1;
		//	projectile.alpha = 255;
			projectile.timeLeft = 5;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
		}

		bool activated = false;
		Vector2 groundpos = Vector2.Zero;
		bool stuck = false;
		public override bool PreAI()
		{
			if (projectile.timeLeft == 1 && !stuck)
			{
				projectile.position += projectile.velocity * 8;
				projectile.velocity = Vector2.Zero;
				stuck = true;
				projectile.timeLeft = 580;
			}
			if (!activated && projectile.timeLeft > 60)
			{
				activated = true;
				groundpos = projectile.Center + new Vector2(0, 40);
				projectile.friendly = true;
			}
			projectile.velocity.X = 0;
			if (!activated) {
				projectile.velocity.Y = 30;
				projectile.timeLeft += 2;
			}
			else if (!stuck)
			{
				Vector2 mouse = new Vector2(projectile.ai[0], projectile.ai[1]);
				Vector2 dir9 = mouse - projectile.Center;
				dir9.Normalize();
				dir9 *= 20;
				projectile.velocity = dir9;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!stuck)
			{
				projectile.timeLeft = (int)(580 * target.knockBackResist);
			}
			target.AddBuff(ModContent.BuffType<Stopped>(), (int)(240 * target.knockBackResist * 1.5f));
			projectile.position += projectile.velocity * 8;
			projectile.velocity = Vector2.Zero;
			stuck = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != projectile.velocity.Y && !activated) {
				activated = true;
				groundpos = projectile.Center + new Vector2(0, 40);
				projectile.friendly = true;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (activated)
			{
				ProjectileExtras.DrawChain(projectile.whoAmI, groundpos - new Vector2(30, 0),
				"SpiritMod/Projectiles/BrambleTrap_Chain");
				ProjectileExtras.DrawChain(projectile.whoAmI, groundpos + new Vector2(30, 0),
				"SpiritMod/Projectiles/BrambleTrap_Chain");
			}
			return false;
		}

	}
}