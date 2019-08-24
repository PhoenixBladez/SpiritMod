using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Polyshot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polyshot");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.light = 0.5f;
			projectile.width = 32;
			projectile.height = 36;
			projectile.friendly = true;
			projectile.damage = 35;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 107);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.life = 0;
			NPC.NewNPC((int)target.position.X, (int)target.position.Y, NPCID.Bunny);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 107);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
