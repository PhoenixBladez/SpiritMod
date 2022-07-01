using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class GraniteRepeaterArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 12;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;

			Projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			if (Projectile.ai[0] == 0)
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			else {
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				int num996 = 15;
				bool flag52 = false;
				bool flag53 = false;
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] % 30f == 0f)
					flag53 = true;
				int num997 = (int)Projectile.ai[1];

				if (!Main.npc[num997].active) {
					NPC target = Main.npc[num997];
					if (Projectile.friendly && !Projectile.hostile) {
						ProjectileExtras.Explode(Projectile.whoAmI, 30, 30,
						delegate {
						});

					}
					SoundEngine.PlaySound(SoundID.Item109);
					{
						for (int i = 0; i < 20; i++) {
							int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[dust].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[dust].scale *= .25f;
							if (Main.dust[dust].position != Projectile.Center)
								Main.dust[dust].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<GraniteSpike1>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
					Main.projectile[proj].timeLeft = 2;
				}
				if (Projectile.localAI[0] >= (float)(60 * num996))
					flag52 = true;
				else if (num997 < 0 || num997 >= 200)
					flag52 = true;
				else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
					Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[num997].gfxOffY;
					if (flag53) {
						Main.npc[num997].HitEffect(0, 1.0);
					}
				}
				else
					flag52 = true;

				if (flag52)
					Projectile.Kill();
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = (float)target.whoAmI;
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
			Projectile.netUpdate = true;
			Projectile.damage = 0;

			int num31 = 1;
			Point[] array2 = new Point[num31];
			int num32 = 0;

			for (int n = 0; n < 1000; n++) {
				if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
					array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
					if (num32 >= array2.Length)
						break;
				}
			}

			if (num32 >= array2.Length) {
				int num33 = 0;
				for (int num34 = 1; num34 < array2.Length; num34++) {
					if (array2[num34].Y < array2[num33].Y)
						num33 = num34;
				}
				Main.projectile[array2[num33].X].Kill();
			}
			if (target.life <= 0) {
				if (Projectile.friendly && !Projectile.hostile) {
					ProjectileExtras.Explode(Projectile.whoAmI, 30, 30,
					delegate {
					});

				}
				SoundEngine.PlaySound(SoundID.Item109);
				{
					for (int i = 0; i < 20; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center.X, target.Center.Y,
					0, 0, ModContent.ProjectileType<GraniteSpike1>(), Projectile.damage / 3 * 2, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = 2;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int k = 0; k < 6; k++) {
				Dust.NewDust(Projectile.GetSource_Death(), Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 2.5f * 1, -2.5f, 0, default, 0.27f);
				Dust.NewDust(Projectile.GetSource_Death(), Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 2.5f * 1, -2.5f, 0, default, 0.37f);
			}
		}
	}
}
