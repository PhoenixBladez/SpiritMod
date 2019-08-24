using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteWall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Wall");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 24;
			projectile.height = 88;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			//Create particles
			int dust = Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				240, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				229, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust1].scale = 2f;
			Main.dust[dust1].noGravity = true;
			return false;
		}

	}
}
