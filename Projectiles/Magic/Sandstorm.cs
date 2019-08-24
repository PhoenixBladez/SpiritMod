using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Sandstorm : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstorm");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			projectile.timeLeft = 30;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = true;
			//Just used the dust code i used for whirlpool lol
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 85, 0f, 0f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 36, 0f, 0f);
			int dust3 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 32, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].scale = 1.5f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust3].scale = 1.5f;
			Main.dust[dust3].noGravity = true;
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 85, 0f, 0f); //to make some with gravity to fly all over the place :P
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, 32, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(mod.BuffType("ClatterPierce"), 180);
		}

	}
}
