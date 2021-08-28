using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mangrove_Defender
{
	public class Earth_Slam_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earth Slam");
		}
		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.height = 8;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.hide = true;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.timeLeft = 1;
		}
		public override void AI()
		{
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 14, 1f, -0.9f);
			for (int index = 0; index < 30; ++index)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 50), projectile.width, projectile.height, 7, 0.0f, 0.0f, 0, new Color(), 1.2f)];
				dust.velocity.Y -= (float)(1.0 + (double)4 * 1.5);
				dust.velocity.Y *= Main.rand.NextFloat();
				dust.scale += (float)8 * 0.03f;
			}
			if (4 >= 2)
			{
				for (int index = 0; index < 40 - 1; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 50), projectile.width, projectile.height, 7, 0.0f, 0.0f, 0, new Color(), 1.2f)];
					dust.velocity.Y -= 1f + (float)3;
					dust.velocity.Y *= Main.rand.NextFloat();
				}
			}
		}
		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			target.velocity.Y -= 10f;
		}
		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
		{
			target.velocity.Y -= 10f;
		}
	}
}