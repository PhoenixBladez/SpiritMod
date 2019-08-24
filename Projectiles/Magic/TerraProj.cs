using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SpiritMod.Projectiles.Magic
{
	public class TerraProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Wrath");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = true;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 74, 0f, 0f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 74, 0f, 0f);
			int dust3 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 74, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 30f, 0f, mod.ProjectileType("TerraProj2"), projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -30f, 0f, mod.ProjectileType("TerraProj2"), projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -30f, mod.ProjectileType("TerraProj2"), projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10f, 30f, mod.ProjectileType("TerraProj2"), projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Ichor, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}

	}
}

