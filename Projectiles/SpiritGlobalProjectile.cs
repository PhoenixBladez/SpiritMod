using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Armor;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{

	public class SpiritGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool stop = false;
		public float xspeed;
		public float yspeed;
		public bool WitherLeaf = false;
		public bool shotFromStellarCrosbow = false;
		public bool shotFromBloodshot = false;
		public bool shotFromCookieCutter = false;
		public bool shotFromGaruda = false;
		public bool shotFromClatterBow = false;
        public bool shotFromBismiteBow = false;
		public bool shotFromThornBow = false;
		public bool shotFromNightSky = false;
		public bool shotFromPalmSword = false;
		public bool shotFromGeodeBow = false;
		public bool shotFromSpazLung = false;
		public bool shotFromCoralBow = false;
		public bool shotFromHolyBurst = false;
		public bool shotFromTrueHolyBurst = false;
		public bool HeroBow1 = false;
		public bool HeroBow2 = false;
		public bool HeroBow3 = false;
		public bool shotFromMarbleBow;
		public float counter = -1440;

		public bool throwerGloveBoost = false;

		public bool shotFromMaliwanFireCommon = false;
		public bool shotFromMaliwanAcidCommon = false;
		public bool shotFromMaliwanShockCommon = false;
		public bool shotFromMaliwanFreezeCommon = false;



		public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if(projectile.minion && projectile.owner == Main.myPlayer && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>())) {
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(20f * sineAdd), (int)(16f * sineAdd), (int)(4f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .25f), SpriteEffects.None, 0f);

			}
			if(throwerGloveBoost && projectile.thrown) {
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for(int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
				return true;
			}
			return base.PreDraw(projectile, spriteBatch, lightColor);
		}
		float alphaCounter;
		public override bool PreAI(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if(projectile.minion && projectile.owner == Main.myPlayer && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>())) {
				alphaCounter += .04f;
			}
			if(throwerGloveBoost) {
				projectile.penetrate = 2;
				projectile.scale = 1.1f;
				ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
				ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			}
			if(shotFromMaliwanFreezeCommon == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for(int k = 0; k < 3; k++) {
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 180);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}
			if(shotFromMaliwanFireCommon == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for(int k = 0; k < 3; k++) {
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 127);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}
			if(shotFromMaliwanAcidCommon == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for(int k = 0; k < 3; k++) {
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 163);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}
			if(shotFromMaliwanShockCommon == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for(int k = 0; k < 3; k++) {
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 226);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .58f;
				}
			}
			if(WitherLeaf == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.Next(2) == 0) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 167);
					return true;
				}
			}

			if(HeroBow1 == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = 1.5f;
					return true;
				}
			}
			if(HeroBow2 == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.Next(2) == 0) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = 1.5f;
					return true;
				}
			}
			if(projectile.minion && projectile.owner == Main.myPlayer && modPlayer.silkenSet) {
				int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.GoldCoin);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[dust].position = projectile.Center - vector2_3;
			}

			if(HeroBow3 == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.Next(2) == 0) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = 1.8f;
					return true;
				}
			}
			if(shotFromStellarCrosbow == true) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.Next(2) == 0)
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 133);
				return false;
			}
            if (shotFromHolyBurst == true || shotFromTrueHolyBurst == true)
            {
                counter++;
                if (counter >= 1440)
                {
                    counter = -1440;
                }
                for (int i = 0; i < 6; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                    int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 112, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num].velocity *= .1f;
                    Main.dust[num].scale *= .7f;
                    Main.dust[num].noGravity = true;

                }
                for (int f = 0; f < 6; f++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)f;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)f;

                    int num = Dust.NewDust(projectile.Center - new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 112, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num].velocity *= .1f;
                    Main.dust[num].scale *= .7f;
                    Main.dust[num].noGravity = true;

                }
                return false;
            }
            else if (shotFromBloodshot == true)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                if (Main.rand.Next(2) == 0)
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
                return false;
            }
            else if (shotFromGeodeBow == true)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
                int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135);
                int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].scale = 1.2f;
                Main.dust[dust1].noGravity = true;
                Main.dust[dust1].velocity *= 0f;
                Main.dust[dust1].scale = 1.2f;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 0f;
                Main.dust[dust2].scale = 1.2f;
                return false;
            }
            else if (shotFromClatterBow == true)
            {
                projectile.magic = false;
                projectile.ranged = true;

                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                int num = 5;
                for (int k = 0; k < 3; k++)
                {
                    int index2 = Dust.NewDust(projectile.position, 1, 1, 147, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                    Main.dust[index2].scale = .5f;
                    Main.dust[index2].velocity *= 0f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = false;
                }
            }
            else if (shotFromMarbleBow == true)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height - 10, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height - 10, 236, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 0f;
                Main.dust[dust2].velocity *= 0f;
                Main.dust[dust2].scale = .5f;
                Main.dust[dust].scale = 2f;
                return false;
            }
            else if (shotFromBismiteBow == true)
            {
                if (Main.rand.Next(20) == 0)
                {
                    DustHelper.DrawTriangle(projectile.Center, 167, 1, .8f, 1.1f);
                }
            }
			return base.PreAI(projectile);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(shotFromMaliwanFreezeCommon == true) {
				if(Main.rand.Next(6) == 0) {
					target.AddBuff(ModContent.BuffType<MageFreeze>(), 120);
					int d = 180;
					int d1 = 180;
					for(int k = 0; k < 20; k++) {
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, .7f);
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, 0.27f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, .9f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, 0.27f);
					}
				}
			}
			if(shotFromMaliwanFireCommon == true) {
				if(Main.rand.Next(6) == 0) {
					target.AddBuff(BuffID.OnFire, Main.rand.Next(120, 180));
					int d = 6;
					int d1 = 6;
					for(int k = 0; k < 20; k++) {
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, .7f);
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, 0.27f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, .9f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, 0.27f);
					}
				}

			}
			if(shotFromMaliwanAcidCommon == true) {
				if(Main.rand.Next(6) == 0) {
					target.AddBuff(BuffID.Poisoned, Main.rand.Next(120, 180));
					int d = 163;
					int d1 = 163;
					for(int k = 0; k < 20; k++) {
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, .7f);
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, 0.27f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, .9f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, 0.27f);
					}
				}
			}
			if(shotFromMaliwanShockCommon == true) {
				if(Main.rand.Next(12) == 0 || player.GetSpiritPlayer().starSet && Main.rand.Next(8) == 0) {
					if(player.GetSpiritPlayer().starSet) {
						int newdamage = damage + (int)(damage * .15f);
						damage = newdamage;
					}
					target.AddBuff(ModContent.BuffType<ElectrifiedV2>(), Main.rand.Next(60, 120));
					int d = 226;
					int d1 = 226;
					for(int k = 0; k < 20; k++) {
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, .7f);
						Dust.NewDust(target.position, target.width, target.height, d, 2.5f, -2.5f, 0, Color.White, 0.27f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, .9f);
						Dust.NewDust(target.position, target.width, target.height, d1, 2.5f, -2.5f, 0, Color.White, 0.27f);
					}
				}
			}
			if(shotFromCoralBow && Main.rand.Next(2) == 0) {
				target.StrikeNPC(projectile.damage / 4, 0f, 0, crit);
				for(int k = 0; k < 20; k++) {
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f * projectile.direction, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f * projectile.direction, -2.5f, 0, default(Color), .34f);
				}
			}
			if(shotFromStellarCrosbow == true) {
				target.AddBuff(ModContent.BuffType<StarFracture>(), 300);
			}
			if(shotFromHolyBurst == true) {
				target.AddBuff(ModContent.BuffType<AngelLight>(), 60);
			}
			if(shotFromTrueHolyBurst == true) {
				target.AddBuff(ModContent.BuffType<AngelWrath>(), 60);
			} else if(shotFromPalmSword == true) {
				target.AddBuff(BuffID.Poisoned, 300);
			} else if(shotFromSpazLung == true) {
				target.AddBuff(BuffID.CursedInferno, 120);
			} else if(shotFromBloodshot == true) {
				target.AddBuff(ModContent.BuffType<BCorrupt>(), 120);
			} else if(shotFromClatterBow == true && Main.rand.Next(6) == 0) {
				target.AddBuff(ModContent.BuffType<ClatterPierce>(), 120);
			} else if(shotFromCookieCutter == true) {
				player.AddBuff(ModContent.BuffType<CrimsonRegen>(), 179);
			} else if(shotFromNightSky == true && Main.rand.Next(8) == 0) {
				target.AddBuff(ModContent.BuffType<StarFlame>(), 179);
			} else if(shotFromThornBow == true && Main.rand.Next(4) == 0) {
				int n = Main.rand.Next(5, 6);
				int deviation = Main.rand.Next(0, 300);
				for(int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 3.5f;
					perturbedSpeed.Y *= 3.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<ThornBowThorn>(), projectile.damage / 5 * 3, projectile.knockBack, projectile.owner);
				}
			} else if(shotFromGaruda == true && Main.rand.Next(2) == 0) {
				int n = Main.rand.Next(1, 2);
				int deviation = Main.rand.Next(0, 300);
				for(int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 0f;
					perturbedSpeed.Y *= 7.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 200, 0, 12, ModContent.ProjectileType<GodHomingProj>(), projectile.damage / 2 * 3, projectile.knockBack, projectile.owner);
				}
			} else if(shotFromGeodeBow == true) {
				if(crit) {
					target.AddBuff(BuffID.CursedInferno, 240, true);
					target.AddBuff(BuffID.Frostburn, 240, true);
					target.AddBuff(BuffID.OnFire, 240, true);
				}
			}

			if(WitherLeaf == true) {
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
			}
			if(modPlayer.HealCloak && projectile.minion && Main.rand.Next(25) == 1) {
				player.HealEffect(4);
				player.statLife += (4);
			}
			if(modPlayer.SpiritCloak && projectile.minion && Main.rand.Next(15) == 1) {
				player.HealEffect(9);
				player.statLife += (9);
			}

			if(modPlayer.VampireCloak && projectile.minion && Main.rand.Next(100) < 30) {
				player.HealEffect(3);
				player.statLife += (3);
			}

			if(HeroBow1 == true) {
				target.AddBuff(BuffID.OnFire, 240, true);

				if(Main.rand.Next(4) == 0) {
					target.AddBuff(BuffID.CursedInferno, 180, true);
				}
				if(Main.rand.Next(8) == 0) {
					target.AddBuff(BuffID.ShadowFlame, 180, true);
				}
			}
			if(HeroBow2 == true) {
				target.AddBuff(BuffID.Frostburn, 120, true);

				if(Main.rand.Next(15) == 0) {
					target.AddBuff(ModContent.BuffType<MageFreeze>(), 180, true);
				}
			}
			if(HeroBow3 == true) {
				if(Main.rand.Next(100) == 2) {
					target.AddBuff(ModContent.BuffType<Death>(), 240, true);
				}
			}
            if (shotFromBismiteBow == true)
            {
                if (Main.rand.Next(5) == 0)
                {
                    target.AddBuff(ModContent.BuffType<FesteringWounds>(), 120, true);
                }
            }
        }
		public override void Kill(Projectile projectile, int timeLeft)
		{
			SpiritMod.TrailManager.TryTrailKill(projectile);
		}
	}
}
