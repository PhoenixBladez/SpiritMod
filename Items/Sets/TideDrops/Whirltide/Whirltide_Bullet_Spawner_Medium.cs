using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide_Bullet_Spawner_Medium : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Whirltide Bullet Spawner");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.hide = true;
			projectile.scale = 1f;
			projectile.timeLeft = 60;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			projectile.velocity.Y = 100f;
			projectile.velocity.X = 0f;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 8, 0f, -3f, mod.ProjectileType("Whirltide_Water_Explosion"), 14, 9f, 0);
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 3));
		}
	}
}