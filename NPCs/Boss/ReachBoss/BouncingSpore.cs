using Microsoft.Xna.Framework;
using Terraria;
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
			projectile.hostile = true;
			projectile.height = 44;
			projectile.width = 42;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.aiStyle = 2;
			projectile.penetrate = 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			float goreScale = 0.01f * Main.rand.Next(20, 70);
			int a = Gore.NewGore(projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), projectile.velocity, 386, goreScale);
			Main.gore[a].timeLeft = 15;
			Main.gore[a].rotation = 10f;
			Main.gore[a].velocity = new Vector2(projectile.direction * 2.5f, Main.rand.NextFloat(1f, 2f));
		
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();


			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * .5f;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * -1f;

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}

		public override void AI()
		{
			projectile.rotation += 0.25f;
			Lighting.AddLight(projectile.Center, .075f, .179f,.084f);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), projectile.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(projectile.direction * 2.5f, Main.rand.NextFloat(1f, 2f));
				
				int a1 = Gore.NewGore(projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), projectile.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(projectile.direction * 2.5f, Main.rand.NextFloat(10f, 20f));
			}
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Plantera_Green, 2.5f * projectile.direction, -2.5f, 0, default, 0.7f);
			}
		}
	}
}