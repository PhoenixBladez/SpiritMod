using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PoisonCloud : ModProjectile
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Cloud");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.tileCollide = true;
			projectile.timeLeft = 180;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI == projectile.ai[0])
				return false;

			return null;
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0f)
			{
				projectile.localAI[0] = 1f;
				projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}

			projectile.velocity *= 0.95f;
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, 0f);
			Main.dust[dust].noGravity = true;
			dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Pestilence"), 0f, 0f);
			Main.dust[dust].noGravity = true;

			projectile.frameCounter++;
			if ((float)projectile.frameCounter >= 12f)
			{
				if(++projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
				projectile.frameCounter = 0;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Poisoned, 300);
		}
	}
}
