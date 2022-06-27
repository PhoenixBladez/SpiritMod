using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class TemporalField : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Temporal Field");

		public override void SetDefaults()
		{
			Projectile.width = 600;       //projectile width
			Projectile.height = 300;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Melee;         // 
			Projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 600;   //how many time projectile projectile has before disepire
			Projectile.light = 0.75f;    // projectile light
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			var rect = new Rectangle((int)Projectile.Center.X, (int)Projectile.position.Y, 200, 200);
			for (int index1 = 0; index1 < Main.maxNPCs; index1++)
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
					Main.npc[index1].AddBuff(ModContent.BuffType<Buffs.Slow>(), 240);
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 40; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}
		}
	}
}
