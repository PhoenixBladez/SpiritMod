using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class GrailWard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Ward");
		}

		public override void SetDefaults()
		{
			projectile.width = 150;       //projectile width
			projectile.height = 150;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.melee = true;         // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 120;   //how many time projectile projectile has before disepire
			projectile.light = 0.75f;    // projectile light
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			int num621 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 100, default(Color), 2f);
			Main.dust[num621].velocity *= 1f;
			if (Main.rand.Next(2) == 0)
			{
				Main.dust[num621].scale = 0.5f;
				Main.dust[num621].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

			Rectangle rect = new Rectangle((int)projectile.Center.X, (int)projectile.position.Y, 150, 150);
			for (int index1 = 0; index1 < 200; index1++)
			{
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
				{
					Main.npc[index1].AddBuff(mod.BuffType("HolyBurn"), 150);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 40; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
		}

		//public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		//{
		//	target.AddBuff(mod.BuffType("Slow"), 240); 
		//	damage = 0;
		//}
	}
}
