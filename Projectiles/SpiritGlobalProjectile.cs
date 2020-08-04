using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Armor;
using SpiritMod.Items;
using SpiritMod.Projectiles.Bullet;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{

	public class SpiritGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public List<SpiritProjectileEffect> effects = new List<SpiritProjectileEffect>();
		public bool stop = false;
		public float xspeed;
		public float yspeed;
		public bool WitherLeaf = false;
		public bool shotFromStellarCrosbow = false;
		public bool shotFromBloodshot = false;
		public bool shotFromCookieCutter = false;
		public bool shotFromGaruda = false;
		public bool shotFromBismiteBow = false;
		public bool shotFromThornBow = false;
		public bool shotFromNightSky = false;
		public bool shotFromPalmSword = false;
		public bool shotFromGeodeBow = false;
		public bool shotFromSpazLung = false;
		public bool shotFromCoralBow = false;
		public bool shotFromHolyBurst = false;
		public bool shotFromTrueHolyBurst = false;
		public bool shotFromNightbane = false;
		public bool shotFromMarbleBow;
		public float counter = -1440;

		public bool throwerGloveBoost = false;
		public bool runOnce = false;

		public bool shotFromMaliwanFireCommon = false;
		public bool shotFromMaliwanAcidCommon = false;
		public bool shotFromMaliwanShockCommon = false;
		public bool shotFromMaliwanFreezeCommon = false;

		public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if(projectile.minion && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>())) {
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, new Color((int)(20f * sineAdd), (int)(16f * sineAdd), (int)(4f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .25f), SpriteEffects.None, 0f);
			}
			if(throwerGloveBoost && projectile.thrown) {
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for(int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
				return true;
			}
			return base.PreDraw(projectile, spriteBatch, lightColor);
		}

		float alphaCounter;
		public override bool PreAI(Projectile projectile)
		{
			foreach(var effect in effects) {
				if(!effect.ProjectilePreAI(projectile))
					return false;
			}

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if(projectile.friendly
				&& projectile.owner != 255
				&& projectile.ranged
				&& modPlayer.throwerGlove
				&& modPlayer.throwerStacks >= 7
				&& !runOnce) {
				modPlayer.firedSharpshooter = true;
				projectile.extraUpdates += 1;
				projectile.scale *= 1.1f;
				projectile.damage += (int)(projectile.damage / 4f + 0.5f);
				projectile.knockBack += 2;
				throwerGloveBoost = true;
			}

			runOnce = true;

			if(projectile.minion && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>())) {
				alphaCounter += .04f;
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
			if(WitherLeaf) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.NextBool()) {
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 167);
					return true;
				}
			}
			if(projectile.minion && modPlayer.silkenSet) {
				int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.GoldCoin);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vel = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
				vel.Normalize();
				Main.dust[dust].velocity = vel * Main.rand.Next(50, 100) * 0.04f;
				Main.dust[dust].position = projectile.Center - vel * 34f;
			}
			if(shotFromStellarCrosbow) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if(Main.rand.Next(2) == 0)
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 133);
				return false;
			}
			if(shotFromHolyBurst || shotFromTrueHolyBurst) {
				counter++;
				if(counter >= 1440) {
					counter = -1440;
				}
				for(int i = 0; i < 2; i++) {
					int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 112, 0f, 0f, 0, default, 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;
				}
				for(int i = 0; i < 2; i++) {
					int num = Dust.NewDust(projectile.Center - new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 112, 0f, 0f, 0, default, 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;
				}
				return false;
			} else if(shotFromBloodshot) {
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
				if(Main.rand.Next(2) == 0)
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
				return false;
			} else if(shotFromGeodeBow) {
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, Scale: 1.2f);
				int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, Scale: 1.2f);
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, Scale: 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust1].noGravity = true;
				Main.dust[dust1].velocity *= 0f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 0f;
				return false;
			} else if(shotFromMarbleBow) {
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
			} else if(shotFromBismiteBow) {
				if(Main.rand.Next(20) == 0) {
					DustHelper.DrawTriangle(projectile.Center, 167, 1, .8f, 1.1f);
				}
			}
			return base.PreAI(projectile);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			foreach(var effect in effects)
				effect.ProjectileOnHitNPC(projectile, target, damage, knockback, crit);

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(shotFromMaliwanFreezeCommon) {
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
			if(shotFromMaliwanFireCommon) {
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
			if(shotFromMaliwanAcidCommon) {
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
			if(shotFromMaliwanShockCommon) {
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
			if(shotFromCoralBow && Main.rand.NextBool()) {
				target.StrikeNPC(projectile.damage / 4, 0f, 0, crit);
				for(int k = 0; k < 20; k++) {
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f * projectile.direction, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f * projectile.direction, -2.5f, 0, default, 0.34f);
				}
			}
			if(shotFromStellarCrosbow) {
				target.AddBuff(ModContent.BuffType<StarFracture>(), 300);
			}
			if(shotFromHolyBurst) {
				target.AddBuff(ModContent.BuffType<AngelLight>(), 60);
			}
			if(shotFromTrueHolyBurst) {
				target.AddBuff(ModContent.BuffType<AngelWrath>(), 60);
			} else if(shotFromPalmSword) {
				target.AddBuff(BuffID.Poisoned, 300);
			} else if(shotFromSpazLung) {
				target.AddBuff(BuffID.CursedInferno, 120);
			} else if(shotFromBloodshot) {
				target.AddBuff(ModContent.BuffType<BCorrupt>(), 120);
			} else if(shotFromCookieCutter) {
				player.AddBuff(ModContent.BuffType<CrimsonRegen>(), 179);
			} else if(shotFromNightSky && Main.rand.NextBool(8)) {
				target.AddBuff(ModContent.BuffType<StarFlame>(), 179);
			} else if(shotFromGaruda && Main.rand.NextBool()) {
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
			} else if(shotFromGeodeBow) {
				if(crit) {
					target.AddBuff(BuffID.CursedInferno, 240, true);
					target.AddBuff(BuffID.Frostburn, 240, true);
					target.AddBuff(BuffID.OnFire, 240, true);
				}
			}

			if(WitherLeaf) {
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
			}
			if(modPlayer.HealCloak && projectile.minion && Main.rand.NextBool(25)) {
				player.HealEffect(4);
				player.statLife += 4;
			}
			if(modPlayer.SpiritCloak && projectile.minion && Main.rand.NextBool(15)) {
				player.HealEffect(9);
				player.statLife += 9;
			}

			if(modPlayer.VampireCloak && projectile.minion && Main.rand.Next(100) < 30) {
				player.HealEffect(3);
				player.statLife += 3;
			}
			if(shotFromBismiteBow) {
				if(Main.rand.NextBool(5)) {
					target.AddBuff(ModContent.BuffType<FesteringWounds>(), 120, true);
				}
			}
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			if(Main.netMode != NetmodeID.Server) SpiritMod.TrailManager.TryTrailKill(projectile);
		}
	}
}
