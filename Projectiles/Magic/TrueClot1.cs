using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TrueClot1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Clot");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
			int dust3 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		int timer = 20;

		public override void AI()
		{
			timer--;

			if (timer == 0)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.GoldenShowerFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 20;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 8)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > 5)
					Projectile.frame = 0;

			}
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f)
			{
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 2)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}

			++Projectile.localAI[1];
			int minRadius = 1;
			int minSpeed = 1;

			if (Projectile.localAI[1] <= 1.0)
			{
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, minRadius, minSpeed, ModContent.ProjectileType<TrueClot2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
				Main.projectile[proj].localAI[0] = Projectile.whoAmI;
			}
			else
			{
				switch ((int)Projectile.localAI[1])
				{
					case 10:
						minSpeed -= 1;
						break;
					case 30:
						minSpeed -= 1;
						break;
					case 50:
						minSpeed -= 1;
						break;
					case 70:
						minSpeed -= 1;
						break;
				}

				if ((int)Projectile.localAI[1] == 120)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, minRadius, minSpeed, ModContent.ProjectileType<TrueClot2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = Projectile.whoAmI;
				}

				if ((int)Projectile.localAI[1] == 180)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, minRadius, minSpeed, ModContent.ProjectileType<TrueClot2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = Projectile.whoAmI;
				}

				if ((int)Projectile.localAI[1] == 240)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, minRadius, minSpeed, ModContent.ProjectileType<TrueClot2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = Projectile.whoAmI;
				}

				if ((int)Projectile.localAI[1] == 300)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, minRadius, minSpeed, ModContent.ProjectileType<TrueClot2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = Projectile.whoAmI;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) <= 1)
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, Projectile.owner, Projectile.owner, Main.rand.Next(1, 2));

			if (Main.rand.NextBool(3))
				target.AddBuff(BuffID.Ichor, 300, true);
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 30f, 0f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -30f, 0f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -30f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 10f, 30f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -10f, 30f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X - 100, Projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X - -100, Projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<Blood3>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
		}
	}
}