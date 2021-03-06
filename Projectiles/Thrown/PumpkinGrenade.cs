using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class PumpkinGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Grenade");
		}

		public override void SetDefaults()
		{
			///for reasons, I have to put a comment here.
			projectile.aiStyle = 16;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.hostile = false;
			projectile.timeLeft = 180;
			projectile.width = 20;
			projectile.height = 20;
		}

		public override void Kill(int timeLeft)
		{
			int proj = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ModContent.ProjectileType<PumpkinBoom>(), (int)(projectile.damage), 0, Main.myPlayer);
			//Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 12);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
		}
	}
}