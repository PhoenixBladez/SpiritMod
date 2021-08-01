using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.PigronStaff
{
	[AutoloadMinionBuff("Pigrons", "Bacon!")]
	public class PigronMinion : BaseMinion
	{
		public PigronMinion() : base(800, 1800, new Vector2(30, 30)) { }

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Pigron");
			Main.projFrames[projectile.type] = 7;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = 14;
			return true;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(600) == 0 && Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.Zombie, (int)projectile.Center.X, (int)projectile.Center.Y, Main.rand.Next(39, 41), 0.33f, 0.5f);

			return true;
		}

		private int BiomeType => IndexOfType % 3;

		public override void IdleMovement(Player player)
		{
			projectile.direction = projectile.spriteDirection = (projectile.Center.X < player.MountedCenter.X) ? -1 : 1;
			Vector2 targetCenter = player.MountedCenter - new Vector2(50 * (IndexOfType + 1) * player.direction, 50 + (float)(Math.Sin((Main.GameUpdateCount / 8f) + IndexOfType) * 6));
			projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(targetCenter) * 
				MathHelper.Clamp(projectile.Distance(targetCenter) * (float)Math.Pow((float)IndexOfType / (player.ownedProjectileCounts[projectile.type] + 1) + 1, 2) / 20, 3, 24), 0.03f);

			projectile.rotation = Utils.AngleLerp(projectile.velocity.X * 0.05f, projectile.velocity.ToRotation() + (projectile.direction > 0 ? MathHelper.Pi : 0), 
				MathHelper.Clamp((projectile.Distance(targetCenter) - 200) / 200f, 0, 1f));

			if (projectile.Distance(targetCenter) > 1800)
			{
				projectile.Center = targetCenter;
				projectile.netUpdate = true;
			}

			projectile.ai[0] = 0;
			projectile.ai[1] = -1;
			projectile.alpha = Math.Max(projectile.alpha - 8, 0);
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			projectile.ai[0] = 1;
			projectile.direction = projectile.spriteDirection = (projectile.velocity.X < 1) ? 1 : -1;
			projectile.rotation = projectile.velocity.ToRotation() + (projectile.direction > 0 ? MathHelper.Pi : 0);
			if (Main.rand.Next(9) == 0 && projectile.velocity.Length() > 7)
			{
				int dustID = 0;
				switch (BiomeType)
				{
					case 0:
						dustID = DustID.CrystalPulse2;
						break;
					case 1:
						dustID = 112;
						break;
					case 2:
						dustID = 114;
						break;
				}
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, dustID, projectile.velocity.X / 3, projectile.velocity.Y / 3, 100, default, Main.rand.NextFloat(0.7f, 1.2f));
				dust.fadeIn = 0.8f;
				dust.noGravity = true;
			}

			switch (projectile.ai[1])
			{
				case -1:
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 12, 0.1f);
					goto case 4;
				case 0:
				case 2:
				case 4:
					projectile.alpha += Math.Max(16 - IndexOfType / 2, 12);
					projectile.velocity *= 0.97f;
					if (projectile.alpha >= 255)
					{
						projectile.ai[1]++;
						projectile.alpha = 255;
						projectile.Center = target.Center + target.DirectionTo(player.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(350, 400);
						projectile.velocity = projectile.DirectionTo(target.Center) * Main.rand.NextFloat(11, 16);
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.DD2_WyvernDiveDown.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);
						projectile.netUpdate = true;
					}
					break;
				case 1:
				case 3:
				case 5:
					projectile.velocity *= 1.03f;
					projectile.alpha -= Math.Max(16 - IndexOfType / 2, 12);
					if (projectile.alpha <= 0)
					{
						projectile.alpha = 0;
						projectile.netUpdate = true;
						projectile.ai[1]++;
					}
					break;
				case 6:
					if (++projectile.localAI[0] == 10)
					{
						projectile.localAI[1] = (Main.rand.NextBool()? 1 : -1);
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.DD2_WyvernDiveDown.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);

						projectile.netUpdate = true;
					}

					if(projectile.localAI[0] > 10)
					{
						projectile.velocity = projectile.velocity.RotatedBy(projectile.localAI[1] * MathHelper.TwoPi / 20);
						if(projectile.localAI[0] % 7 == 0){
							Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PigronBubble>(), projectile.damage / 3, projectile.knockBack, projectile.owner, target.whoAmI, BiomeType);
							projectile.netUpdate = true;
						}
					}

					if (projectile.localAI[0] >= 40)
						projectile.ai[1]++;
					break;
				default: 
					projectile.ai[1] = 0;
					projectile.localAI[0] = 0;
					break;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D drawTex = ModContent.GetTexture(Texture);
			Color trailColor = Color.Pink;
			switch (BiomeType) {
				case 1:
					trailColor = Color.Purple;
					drawTex = ModContent.GetTexture(Texture + "_corrupt");
					break;
				case 2:
					trailColor = Color.Red;
					drawTex = ModContent.GetTexture(Texture + "_crim");
					break;
			}

			if (projectile.ai[0] == 1) { 
				for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
				{
					float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
					opacity *= 0.5f * projectile.Opacity;
					spriteBatch.Draw(drawTex, projectile.oldPos[i] + (projectile.Size / 2) - Main.screenPosition, projectile.DrawFrame(), trailColor * opacity, projectile.oldRot[i], 
						projectile.DrawFrame().Size() / 2, projectile.scale, (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				}
			}

			spriteBatch.Draw(drawTex, projectile.Center - Main.screenPosition, projectile.DrawFrame(), lightColor * projectile.Opacity, projectile.rotation, projectile.DrawFrame().Size() / 2, 
				projectile.scale, (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}

	internal class PigronBubble : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pigron Bubble");
			ProjectileID.Sets.MinionShot[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(20, 20);
			projectile.scale = Main.rand.NextFloat(0.5f, 1f);
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 60;
			projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			projectile.hide = true;
		}

		private int BiomeType => (int)projectile.ai[1];

		public override void AI()
		{
			NPC target = Main.npc[(int)projectile.ai[0]];
			if (!target.active || !target.CanBeChasedBy(this))
			{
				projectile.Kill();
				return;
			}
			projectile.rotation += (projectile.velocity.X < 0) ? -0.15f : 0.15f;
			projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 16, 0.05f);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.Item54.WithPitchVariance(0.3f), projectile.Center);

			for(int i = 0; i < 12; i++)
			{
				int dustID = 0;
				switch (BiomeType)
				{
					case 0:
						dustID = DustID.CrystalPulse2;
						break;
					case 1:
						dustID = 112;
						break;
					case 2:
						dustID = 114;
						break;
				}

				Dust dust = Dust.NewDustPerfect(projectile.Center, dustID, Main.rand.NextVector2Circular(5, 5), 50, default, (projectile.scale / 3) * Main.rand.NextFloat(0.7f, 1.3f));
				dust.noGravity = true;
				dust.fadeIn = 0.4f;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 10);

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color drawColor = new Color(255, 99, 229);
			switch (BiomeType)
			{
				case 1:
					drawColor = new Color(131, 8, 255);
					break;
				case 2:
					drawColor = Color.Red;
					break;
			}
			float glowscale = (float)(Math.Sin(Main.GlobalTime * 4) / 5 + 1);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, drawColor, projectile.rotation, tex.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, drawColor * 0.75f, projectile.rotation, tex.Size() / 2, projectile.scale * glowscale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, drawColor * 0.75f, projectile.rotation, tex.Size() / 2, projectile.scale * (1/glowscale), SpriteEffects.None, 0);

			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Lerp(drawColor, Color.White, 0.25f) * 0.8f, projectile.rotation, bloom.Size() / 2, projectile.scale/3.5f, SpriteEffects.None, 0);
		}
	}
}