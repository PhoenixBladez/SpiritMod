using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Artifact
{
	public class Miasma : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miasma");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 1;
			projectile.thrown = true;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 180;
		}

		public override bool PreAI()
		{
			projectile.velocity *= 0.95f;
			projectile.tileCollide = true;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, 0f, 0f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Pestilence"), 0f, 0f);
			Main.dust[dust].scale = 0.8f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].scale = 0.8f;
			Main.dust[dust2].noGravity = true;
			return true;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 8)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Poisoned, 300);
		}

	}
}
