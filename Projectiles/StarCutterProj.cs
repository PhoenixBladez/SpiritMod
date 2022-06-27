using Microsoft.Xna.Framework;
using SpiritMod.Buffs.DoT;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class StarCutterProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Cutter");

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 26;
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			AIType = ProjectileID.Shuriken;
			Projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			if (Projectile.ai[0] == 0)
				Projectile.rotation = Projectile.velocity.ToRotation() + 1.57F;
			else
			{
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				bool flag53 = false;
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] % 30f == 0f)
					flag53 = true;

				int num997 = (int)Projectile.ai[1];
				if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage)
				{
					Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[num997].gfxOffY;

					if (flag53)
						Main.npc[num997].HitEffect(0, 1.0);
				}
				else
					Projectile.Kill();
			}

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = (float)target.whoAmI;
			target.AddBuff(ModContent.BuffType<DoomDestiny>(), 9000, false);
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
			Projectile.netUpdate = true;
			Projectile.damage = 0;
			int num31 = 6;
			Point[] array2 = new Point[num31];
			int num32 = 0;

			for (int n = 0; n < 1000; n++)
			{
				if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI)
				{
					array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
					if (num32 >= array2.Length)
						break;
				}
			}
			if (num32 >= array2.Length)
			{
				int num33 = 0;
				for (int num34 = 1; num34 < array2.Length; num34++)
					if (array2[num34].Y < array2[num33].Y)
						num33 = num34;
				Main.projectile[array2[num33].X].Kill();
			}
		}

		public override void AI()
		{
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
			Projectile.rotation += 0.3f;

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
					if (num416 > 3)
					{
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
		}
	}
}