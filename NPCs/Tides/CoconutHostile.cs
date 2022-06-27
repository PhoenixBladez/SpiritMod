using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class CoconutHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//  Projectile.NewProjectile(projectile.Center, new Vector2(3,0), ModContent.ProjectileType<CoconutSpurtShard>(), projectile.damage, 0, Main.myPlayer);
			//   Projectile.NewProjectile(projectile.Center, new Vector2(-3,0), ModContent.ProjectileType<CoconutSpurtShard>(), projectile.damage, 0, Main.myPlayer);
			Vector2 GoreVel = Projectile.velocity;
			GoreVel.X = 2f;
			GoreVel.Y *= -0.2f;
			Gore.NewGore(Projectile.position, GoreVel, Mod.Find<ModGore>("Gores/Coconut/CoconutSpurtGore").Type, 1f);
			GoreVel.X = -2f;
			Gore.NewGore(Projectile.position, GoreVel, Mod.Find<ModGore>("Gores/Coconut/CoconutSpurtGore").Type, 1f);
			return true;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 14;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.height = 14;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 18);
		}
	}
}
