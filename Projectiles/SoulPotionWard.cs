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
	public class SoulPotionWard : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Guard");
		}

		public override void SetDefaults()
		{
			projectile.width = 300;       //projectile width
			projectile.height = 300;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.melee = true;         // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
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
			Rectangle rect = new Rectangle((int)projectile.Center.X, (int)projectile.position.Y, 300, 300);
			for (int index1 = 0; index1 < 200; index1++)
			{
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
					Main.npc[index1].AddBuff(mod.BuffType("SoulBurn"), 240);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		//public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		//{
		//	target.AddBuff(mod.BuffType("Slow"), 240); 
		//	damage = 0;
		//}
	}
}
