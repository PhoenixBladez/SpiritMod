using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide_Bullet : ModProjectile
	{
		public int timer = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Whirltide Bullet");

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.hide = true;
			Projectile.scale = 1f;
			Projectile.timeLeft = 60;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			timer++;

			if (timer == 20)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y - 36, 0f, 0f, ModContent.ProjectileType<Whirltide_Bullet_Spawner>(), 0, 0f, 0);
			else if (timer == 30)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y - 36, 0f, 0f, ModContent.ProjectileType<Whirltide_Bullet_Spawner_2>(), 0, 0f, 0);
			else if (timer == 40)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y - 36, 0f, 0f, ModContent.ProjectileType<Whirltide_Bullet_Spawner_Medium>(), 0, 0f, 0);
			else if (timer == 50)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y - 36, 0f, 0f, ModContent.ProjectileType<Whirltide_Bullet_Spawner_Medium_2>(), 0, 0f, 0);
			else if (timer == 59)
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y - 36, 0f, 0f, ModContent.ProjectileType<Whirltide_Bullet_Spawner_Large>(), 0, 0f, 0);
		}
	}
}