using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class TerraBullet1 : ModProjectile
	{
		public override void SetStaticDefaults() 
			=> DisplayName.SetDefault("Energy Bolt");

		public override void SetDefaults()
		{
			projectile.width = 4;       //projectile width
			projectile.height = 4;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.ranged = true;         // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 2;      //how many npc will penetrate
			projectile.timeLeft = 300;   //how many time projectile projectile has before disepire // projectile light
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.aiStyle = -1;
			projectile.hide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if(projectile.alpha < 170) {
				for(int i = 0; i < 10; i++) {
					float x = projectile.position.X - 3 - projectile.velocity.X / 10f * i;
					float y = projectile.position.Y - 3 - projectile.velocity.Y / 10f * i;
					int num = Dust.NewDust(new Vector2(x, y), 2, 2, 68);
					Main.dust[num].alpha = projectile.alpha;
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}
			}
			projectile.alpha = Math.Max(0, projectile.alpha - 25);

			bool flag25 = false;
			int jim = 1;
			for(int index1 = 0; index1 < 200; index1++) {
				if(Main.npc[index1].CanBeChasedBy(projectile, false) 
					&& projectile.Distance(Main.npc[index1].Center) < 500 
					&& Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					flag25 = true;
					jim = index1;
				}
			}

			if(flag25) {
				float num1 = 6f;
				Vector2 vector2 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt((double)num2 * num2 + num3 * num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				projectile.velocity.X = (projectile.velocity.X * (num8 - 1) + num6) / num8;
				projectile.velocity.Y = (projectile.velocity.Y * (num8 - 1) + num7) / num8;
			}
		}
	}
}
