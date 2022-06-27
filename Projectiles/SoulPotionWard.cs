using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

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
			Projectile.width = 300;       //projectile width
			Projectile.height = 300;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Melee;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 120;   //how many time projectile projectile has before disepire
			Projectile.light = 0.75f;    // projectile light
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Rectangle rect = new Rectangle((int)Projectile.Center.X, (int)Projectile.position.Y, 300, 300);
			for (int index1 = 0; index1 < 200; index1++) {
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
					Main.npc[index1].AddBuff(ModContent.BuffType<SoulBurn>(), 240);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		//public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		//{
		//	target.AddBuff(ModContent.BuffType<Slow>(), 240); 
		//	damage = 0;
		//}
	}
}
