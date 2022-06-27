using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class FriendlyFeeder : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rotten Meat");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void AI()
		{
			Projectile.rotation += .1f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f)
			{
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[i].ai[1] > num418)
						{
							num417 = i;
							num418 = Main.projectile[i].ai[1];
						}
					}
				}
				if (num416 > 11)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			int num = 5;
			for (int k = 0; k < 2; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int k = 0; k < 16; k++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			return true;
		}

		public class FriendlyFeeder2 : FriendlyFeeder
		{
		}

		public class FriendlyFeeder3 : FriendlyFeeder
		{
		}
	}
}
