using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplateGlove
{
	public class StargloveChargePurple : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfall");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			projectile.hide = false;
			projectile.extraUpdates = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 20;
			projectile.ignoreWater = true;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			DustHelper.DrawStar(projectile.Center, 111, 5, 1.5f,1,1,1,0.5f,true);
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (projectile.velocity.X < 0f)
				projectile.spriteDirection = -1;
			else
				projectile.spriteDirection = 1;
			++projectile.ai[1];
			bool flag2 = (double) Vector2.Distance(projectile.Center, player.Center) > (double) 0f && (double) projectile.Center.Y == (double) player.Center.Y;
			if ((double) projectile.ai[1] >= (double) 30f && flag2)
			{
				projectile.ai[1] = 0.0f;
			}
			
			float num = 3f;
			for (int index1 = 0; (double)index1 < (double)num; ++index1)
			{
				int index2 = Dust.NewDust(projectile.position, 1, 1, 111, 0.0f, 0.0f, 0, Color.Purple, 1.8f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * 5f * (float)index1;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].alpha = 125;
				Main.dust[index2].scale = 0.8f;
			}
		}
	}
}