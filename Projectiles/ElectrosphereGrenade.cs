using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ElectrosphereGrenade : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Electrosphere Grenade");

		public override void SetDefaults()
		{
			Projectile.aiStyle = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 180;
			Projectile.width = 20;
			Projectile.height = 20;
		}

		public override void Kill(int timeLeft)
		{
			int proj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileID.Electrosphere, Projectile.damage, 0, Main.myPlayer);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].timeLeft = 180;

			SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.Kill();
	}
}