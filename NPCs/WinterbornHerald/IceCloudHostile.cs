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
			projectile.hostile = true;
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 500;
		}
		public bool specialKill;
		public override bool PreAI()
		{
			projectile.tileCollide = false;
			{
				int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0.0f, 0.0f, 200, new Color(), 0.5f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity *= 0.75f;
				Main.dust[index].fadeIn = 1.3f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[index].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[index].position = projectile.Center - vector2_3;
			}
			return true;
		}

		int timer = 50;

		public override void AI()
		{
			timer--;
			if (timer <= 0) {
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, Main.rand.Next(5, 9), ProjectileID.FrostShard, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 20;
			}
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
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
