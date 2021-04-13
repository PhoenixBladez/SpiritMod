using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
	public class Visual_Talon : ModProjectile
	{
		float circleTimer = 0f;
		int dustTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purity Light");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.hide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.ignoreWater = true;
			projectile.timeLeft = 180;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			Player p = Main.player[projectile.owner];	
			
			float num2 = 30f;		
			++dustTimer;
			float num3 = dustTimer / num2;
			Vector2 spinningpoint = new Vector2(10f, -10f);
			spinningpoint = spinningpoint.RotatedBy((double)num3 * 1.5 * 6.28318548202515, new Vector2()) * new Vector2(0.25f, 0.25f);
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0.9f;
				}
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(projectile.Center, 0, 0, 228, 0.0f, 0.0f, 100, new Color(), 0.9f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].fadeIn = 0.4f;
				Main.dust[index3].position = new Vector2(projectile.Center.X, projectile.Center.Y) + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}		
			
			if (p.ownedProjectileCounts[mod.ProjectileType("Visual_Talon")] > 10)
				projectile.Kill();
			if (!p.channel)
				projectile.Kill();
			
			double deg = (double) circleTimer; 
			double rad = deg * (Math.PI / 180); 
			double dist = 64; 

			projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;

			circleTimer += 2f;
		}
	}
}