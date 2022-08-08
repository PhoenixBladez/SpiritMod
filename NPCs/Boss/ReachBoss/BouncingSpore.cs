using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class BouncingSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Spore");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.height = 44;
			Projectile.width = 42;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = 2;
			Projectile.penetrate = 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			float goreScale = 0.01f * Main.rand.Next(20, 70);
			int a = Gore.NewGore(Projectile.GetSource_Misc("TileHit"), Projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Projectile.velocity, 386, goreScale);
			Main.gore[a].timeLeft = 15;
			Main.gore[a].rotation = 10f;
			Main.gore[a].velocity = new Vector2(Projectile.direction * 2.5f, Main.rand.NextFloat(1f, 2f));
		
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();


			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = oldVelocity.X * .5f;

			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = oldVelocity.Y * -1f;

			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}

		public override void AI()
		{
			Projectile.rotation += 0.25f;
			Lighting.AddLight(Projectile.Center, .075f, .179f,.084f);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Projectile.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(Projectile.direction * 2.5f, Main.rand.NextFloat(1f, 2f));
				
				int a1 = Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Projectile.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(Projectile.direction * 2.5f, Main.rand.NextFloat(10f, 20f));
			}
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 2.5f * Projectile.direction, -2.5f, 0, default, 0.7f);
			}
		}
	}
}