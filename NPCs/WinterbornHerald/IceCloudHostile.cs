using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.WinterbornHerald
{
	public class IceCloudHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Portal");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 500;
		}
		public bool specialKill;
		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			{
				int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0.0f, 0.0f, 200, new Color(), 0.5f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity *= 0.75f;
				Main.dust[index].fadeIn = 1.3f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[index].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[index].position = Projectile.Center - vector2_3;
			}
			return true;
		}

		int timer = 50;

		public override void AI()
		{
			timer--;
			if (timer <= 0) {
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0f, Main.rand.Next(5, 9), ProjectileID.FrostShard, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 20;
			}
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 2) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}
	}
}
