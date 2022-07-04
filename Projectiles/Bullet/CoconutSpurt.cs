using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 14;
			Projectile.penetrate = 1;
			Projectile.height = 14;
			Projectile.extraUpdates = 1;
			Projectile.scale = Main.rand.NextFloat(1, 1.5f);
		}

		public override void Kill(int timeLeft)
		{
			Vector2 GoreVel = Projectile.velocity;
			GoreVel.X = 2f;
			GoreVel.Y *= -0.2f;
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, GoreVel, Mod.Find<ModGore>("SpiritMod/Gores/Coconut/CoconutSpurtGore").Type, Projectile.scale);
			GoreVel.X = -2f;
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, GoreVel, Mod.Find<ModGore>("SpiritMod/Gores/Coconut/CoconutSpurtGore").Type, Projectile.scale);
			SoundEngine.PlaySound(SoundID.NPCHit18, Projectile.Center);
		}
	}
}
