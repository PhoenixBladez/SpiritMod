using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide_Bullet_Spawner : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Whirltide Bullet Spawner");

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 1;
			Projectile.hide = true;
			Projectile.scale = 1f;
			Projectile.timeLeft = 60;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Projectile.velocity.Y = 100f;
			Projectile.velocity.X = 0f;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y - 8, 0f, -2f, ModContent.ProjectileType<Whirltide_Water_Explosion>(), 12, 8f, 0);
			SoundEngine.PlaySound(SoundID.LiquidsWaterLava);
		}
	}
}