using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class CoconutSpurt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Spurt");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 14;
			projectile.penetrate = 1;
			projectile.height = 14;
			projectile.extraUpdates = 1;
			projectile.scale = Main.rand.NextFloat(1, 1.5f);
		}

		public override void Kill(int timeLeft)
		{
			Vector2 GoreVel = projectile.velocity;
			GoreVel.X = 2f;
			GoreVel.Y *= -0.2f;
			Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutSpurtGore"), projectile.scale);
			GoreVel.X = -2f;
			Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutSpurtGore"), projectile.scale);
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 18);
		}
	}
}
