using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 60;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.hide = true;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.timeLeft = 1;
		}
		public override void AI()
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			for (int index = 0; index < 30; ++index)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 50), Projectile.width, Projectile.height, 7, 0.0f, 0.0f, 0, new Color(), 1.2f)];
				dust.velocity.Y -= (float)(1.0 + (double)4 * 1.5);
				dust.velocity.Y *= Main.rand.NextFloat();
				dust.scale += (float)8 * 0.03f;
			}
			if (4 >= 2)
			{
				for (int index = 0; index < 40 - 1; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 50), Projectile.width, Projectile.height, 7, 0.0f, 0.0f, 0, new Color(), 1.2f)];
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