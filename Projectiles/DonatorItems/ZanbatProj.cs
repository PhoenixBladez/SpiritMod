using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.DonatorItems
{
	public class ZanbatProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Beam");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;       //projectile width
			projectile.height = 24;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.melee = true;         // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 2;      //how many npc will penetrate
			projectile.timeLeft = 600;   //how many time projectile projectile has before disepire // projectile light
			projectile.extraUpdates = 0;
			projectile.alpha = 150;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			projectile.rotation += .3f;

			if (Main.rand.Next(5) == 0)
			{
				int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 244, 0f, 0f, 100, default(Color), 2.5f);
				Main.dust[newDust].scale *= 1.1f;
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 0f;
			}
		}

		public override void Kill(int timeLeft)
		{
			int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 244, 0f, 0f, 100, default(Color), 2.5f);
			Main.dust[newDust].scale *= 1.5f;
			Main.dust[newDust].noGravity = true;
			Main.dust[newDust].velocity *= 1.5F;
		}

	}
}
