using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class CoconutHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
			//  Projectile.NewProjectile(projectile.Center, new Vector2(3,0), ModContent.ProjectileType<CoconutSpurtShard>(), projectile.damage, 0, Main.myPlayer);
			//   Projectile.NewProjectile(projectile.Center, new Vector2(-3,0), ModContent.ProjectileType<CoconutSpurtShard>(), projectile.damage, 0, Main.myPlayer);
			Vector2 GoreVel = projectile.velocity;
			GoreVel.X = 2f;
			GoreVel.Y *= -0.2f;
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutSpurtGore"), 1f);
			GoreVel.X = -2f;
			Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutSpurtGore"), 1f);
			return true;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 14;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.height = 14;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 7);
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 2);
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 2);
		}
	}
}
