using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Armor;
using SpiritMod.Buffs.DoT;
using SpiritMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
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
		public bool shotFromBismiteBow = false;
		public bool shotFromHolyBurst = false;
		public bool shotFromTrueHolyBurst = false;
		public float counter = -1440;

		public bool throwerGloveBoost = false;
		public bool runOnce = false;

		public bool shotFromMaliwanFireCommon = false;
		public bool shotFromMaliwanAcidCommon = false;
		public bool shotFromMaliwanShockCommon = false;
		public bool shotFromMaliwanFreezeCommon = false;

		float alphaCounter;

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if (projectile.minion && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>()))
			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, projectile.Center - Main.screenPosition, null, new Color((int)(20f * sineAdd), (int)(16f * sineAdd), (int)(4f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .25f), SpriteEffects.None, 0f);
			}

			if (throwerGloveBoost && projectile.IsThrown())
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[projectile.type].Value.Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
					Main.spriteBatch.Draw(TextureAssets.Projectile[projectile.type].Value, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return true;
		}


		public override bool PreAI(Projectile projectile)
		{
			foreach (var effect in effects)
			{
				if (effect != null && !effect.ProjectilePreAI(projectile))
					return false;
			}

			if (WitherLeaf)
			{
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				if (Main.rand.NextBool())
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Plantera_Green);
					return true;
				}
			}

			CannonEffects(projectile);

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (projectile.owner >= Main.maxPlayers || projectile.owner <= -1) //Check if owner is invalid
				return true;

			if (projectile.friendly && !runOnce)
			{
				if (projectile.IsRanged() && modPlayer.throwerGlove && modPlayer.throwerStacks >= 7) //Thrower glove functionality
				{
					modPlayer.firedSharpshooter = true;
					projectile.extraUpdates += 1;
					projectile.scale *= 1.1f;
					projectile.damage += (int)(projectile.damage / 4f + 0.5f);
					projectile.knockBack += 2;
					throwerGloveBoost = true;
				}
			}

			runOnce = true;

			if (projectile.minion && modPlayer.stellarSet && player.HasBuff(ModContent.BuffType<StellarMinionBonus>()))
				alphaCounter += .04f;

			if (projectile.minion && modPlayer.silkenSet)
			{
				int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.GoldCoin);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				var vel = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)));
				Main.dust[dust].velocity = vel * Main.rand.Next(50, 100) * 0.04f;
				Main.dust[dust].position = projectile.Center - vel * 34f;
			}

			return BowEffects(projectile);
		}

		private bool BowEffects(Projectile projectile)
		{
			if (shotFromHolyBurst || shotFromTrueHolyBurst)
			{
				counter++;
				if (counter >= 1440)
					counter = -1440;

				for (int i = 0; i < 4; i++)
				{
					int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.Clentaminator_Purple, 0f, 0f, 0, default, 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;
				}
				return false;
			}
			else if (shotFromBismiteBow)
			{
				if (Main.rand.NextBool(20))
					DustHelper.DrawTriangle(projectile.Center, 167, 1, .8f, 1.1f);
			}
			return true;
		}

		private void CannonEffects(Projectile projectile)
		{
			if (shotFromMaliwanFreezeCommon)
			{
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for (int k = 0; k < 3; k++)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.DungeonSpirit);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}

			if (shotFromMaliwanFireCommon)
			{
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for (int k = 0; k < 3; k++)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Flare);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}

			if (shotFromMaliwanAcidCommon)
			{
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for (int k = 0; k < 3; k++)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.PoisonStaff);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .78f;
				}
			}

			if (shotFromMaliwanShockCommon)
			{
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				for (int k = 0; k < 3; k++)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Electric);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = .58f;
				}
			}
		}

		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			foreach (var effect in effects)
				effect.ProjectileOnHitNPC(projectile, target, damage, knockback, crit);

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (shotFromMaliwanFreezeCommon)
			{
				if (Main.rand.NextBool(6))
				{
					target.AddBuff(ModContent.BuffType<MageFreeze>(), 120);
					SpamDust(target, DustID.DungeonSpirit);
				}
			}

			if (shotFromMaliwanFireCommon)
			{
				if (Main.rand.NextBool(6))
				{
					target.AddBuff(BuffID.OnFire, Main.rand.Next(120, 180));
					SpamDust(target, DustID.Torch);
				}
			}

			if (shotFromMaliwanAcidCommon)
			{
				if (Main.rand.NextBool(6))
				{
					target.AddBuff(BuffID.Poisoned, Main.rand.Next(120, 180));
					SpamDust(target, DustID.PoisonStaff);
				}
			}

			if (shotFromMaliwanShockCommon)
			{
				if (Main.rand.NextBool(12) || player.GetSpiritPlayer().starSet && Main.rand.NextBool(8))
				{
					target.AddBuff(ModContent.BuffType<ElectrifiedV2>(), Main.rand.Next(60, 120));
					SpamDust(target, DustID.Electric);
				}
			}

			if (shotFromHolyBurst)
				target.AddBuff(ModContent.BuffType<AngelLight>(), 60);

			if (shotFromTrueHolyBurst)
				target.AddBuff(ModContent.BuffType<AngelWrath>(), 60);

			if (WitherLeaf)
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);

			if (modPlayer.HealCloak && projectile.minion && Main.rand.NextBool(25))
			{
				player.HealEffect(4);
				player.statLife += 4;
			}

			if (modPlayer.SpiritCloak && projectile.minion && Main.rand.NextBool(15))
			{
				player.HealEffect(9);
				player.statLife += 9;
			}

			if (modPlayer.VampireCloak && projectile.minion && Main.rand.Next(100) < 30)
			{
				player.HealEffect(3);
				player.statLife += 3;
			}

			if (shotFromBismiteBow && Main.rand.NextBool(5))
					target.AddBuff(ModContent.BuffType<FesteringWounds>(), 120, true);
		}

		private void SpamDust(Entity target, int dustID)
		{
			for (int k = 0; k < 26; k++)
			{
				Dust.NewDust(target.position, target.width, target.height, dustID, 2.5f, -2.5f, 0, Color.White, .7f);
				Dust.NewDust(target.position, target.width, target.height, dustID, 2.5f, -2.5f, 0, Color.White, 0.27f);
				Dust.NewDust(target.position, target.width, target.height, dustID, 2.5f, -2.5f, 0, Color.White, .9f);
			}
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				Mechanics.Trails.TrailManager.TryTrailKill(projectile);
		}
	}
}