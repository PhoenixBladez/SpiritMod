using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ElectrosphereGrenade : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Electrosphere Grenade");

		public override void SetDefaults()
		{
			projectile.aiStyle = 16;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 180;
			projectile.width = 20;
			projectile.height = 20;
		}

		public override void Kill(int timeLeft)
		{
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ProjectileID.Electrosphere, projectile.damage, 0, Main.myPlayer);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].timeLeft = 180;

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 12);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => projectile.Kill();
	}
}