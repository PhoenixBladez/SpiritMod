using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			Main.projFrames[Projectile.type] = 7;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = 14;
			return true;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(600) == 0 && Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(Main.rand.NextBool() ? SoundID.Zombie39 : SoundID.Zombie40, Projectile.Center);

			return true;
		}

		private int BiomeType => IndexOfType % 3;

		public override void IdleMovement(Player player)
		{
			Projectile.direction = Projectile.spriteDirection = (Projectile.Center.X < player.MountedCenter.X) ? -1 : 1;
			Vector2 targetCenter = player.MountedCenter - new Vector2(50 * (IndexOfType + 1) * player.direction, 50 + (float)(Math.Sin((Main.GameUpdateCount / 8f) + IndexOfType) * 6));
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetCenter) * 
				MathHelper.Clamp(Projectile.Distance(targetCenter) * (float)Math.Pow((float)IndexOfType / (player.ownedProjectileCounts[Projectile.type] + 1) + 1, 2) / 20, 3, 24), 0.03f);

			Projectile.rotation = Utils.AngleLerp(Projectile.velocity.X * 0.05f, Projectile.velocity.ToRotation() + (Projectile.direction > 0 ? MathHelper.Pi : 0), 
				MathHelper.Clamp((Projectile.Distance(targetCenter) - 200) / 200f, 0, 1f));

			if (Projectile.Distance(targetCenter) > 1800)
			{
				Projectile.Center = targetCenter;
				Projectile.netUpdate = true;
			}

			Projectile.ai[0] = 0;
			Projectile.ai[1] = -1;
			Projectile.alpha = Math.Max(Projectile.alpha - 8, 0);
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			Projectile.ai[0] = 1;
			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X < 1) ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction > 0 ? MathHelper.Pi : 0);
			if (Main.rand.Next(9) == 0 && Projectile.velocity.Length() > 7)
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
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustID, Projectile.velocity.X / 3, Projectile.velocity.Y / 3, 100, default, Main.rand.NextFloat(0.7f, 1.2f));
				dust.fadeIn = 0.8f;
				dust.noGravity = true;
			}

			switch (Projectile.ai[1])
			{
				case -1:
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 12, 0.1f);
					goto case 4;
				case 0:
				case 2:
				case 4:
					Projectile.alpha += Math.Max(16 - IndexOfType / 2, 12);
					Projectile.velocity *= 0.97f;
					if (Projectile.alpha >= 255)
					{
						Projectile.ai[1]++;
						Projectile.alpha = 255;
						Projectile.Center = target.Center + target.DirectionTo(player.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(350, 400);
						Projectile.velocity = Projectile.DirectionTo(target.Center) * Main.rand.NextFloat(11, 16);
						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown with { PitchVariance = 0.3f, Volume = 0.5f }, Projectile.Center);
						Projectile.netUpdate = true;
					}
					break;
				case 1:
				case 3:
				case 5:
					Projectile.velocity *= 1.03f;
					Projectile.alpha -= Math.Max(16 - IndexOfType / 2, 12);
					if (Projectile.alpha <= 0)
					{
						Projectile.alpha = 0;
						Projectile.netUpdate = true;
						Projectile.ai[1]++;
					}
					break;
				case 6:
					if (++Projectile.localAI[0] == 10)
					{
						Projectile.localAI[1] = (Main.rand.NextBool()? 1 : -1);
						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown with { PitchVariance = 0.3f, Volume = 0.5f }, Projectile.Center);

						Projectile.netUpdate = true;
					}

					if(Projectile.localAI[0] > 10)
					{
						Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.localAI[1] * MathHelper.TwoPi / 20);
						if(Projectile.localAI[0] % 7 == 0){
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PigronBubble>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, target.whoAmI, BiomeType);
							Projectile.netUpdate = true;
						}
					}

					if (Projectile.localAI[0] >= 40)
						Projectile.ai[1]++;
					break;
				default: 
					Projectile.ai[1] = 0;
					Projectile.localAI[0] = 0;
					break;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D drawTex = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Color trailColor = Color.Pink;
			switch (BiomeType) {
				case 1:
					trailColor = Color.Purple;
					drawTex = ModContent.Request<Texture2D>(Texture + "_corrupt", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
					break;
				case 2:
					trailColor = Color.Red;
					drawTex = ModContent.Request<Texture2D>(Texture + "_crim", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
					break;
			}

			if (Projectile.ai[0] == 1) { 
				for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
				{
					float opacity = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
					opacity *= 0.5f * Projectile.Opacity;
					Main.spriteBatch.Draw(drawTex, Projectile.oldPos[i] + (Projectile.Size / 2) - Main.screenPosition, Projectile.DrawFrame(), trailColor * opacity, Projectile.oldRot[i], 
						Projectile.DrawFrame().Size() / 2, Projectile.scale, (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				}
			}

			Main.spriteBatch.Draw(drawTex, Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), lightColor * Projectile.Opacity, Projectile.rotation, Projectile.DrawFrame().Size() / 2, 
				Projectile.scale, (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}

	internal class PigronBubble : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pigron Bubble");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(20, 20);
			Projectile.scale = Main.rand.NextFloat(0.5f, 1f);
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			Projectile.hide = true;
		}

		private int BiomeType => (int)Projectile.ai[1];

		public override void AI()
		{
			NPC target = Main.npc[(int)Projectile.ai[0]];
			if (!target.active || !target.CanBeChasedBy(this))
			{
				Projectile.Kill();
				return;
			}
			Projectile.rotation += (Projectile.velocity.X < 0) ? -0.15f : 0.15f;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 16, 0.05f);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(SoundID.Item54 with { PitchVariance = 0.3f }, Projectile.Center);

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

				Dust dust = Dust.NewDustPerfect(Projectile.Center, dustID, Main.rand.NextVector2Circular(5, 5), 50, default, (Projectile.scale / 3) * Main.rand.NextFloat(0.7f, 1.3f));
				dust.noGravity = true;
				dust.fadeIn = 0.4f;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 10);

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
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
			float glowscale = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 4) / 5 + 1);
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, drawColor * 0.75f, Projectile.rotation, tex.Size() / 2, Projectile.scale * glowscale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, drawColor * 0.75f, Projectile.rotation, tex.Size() / 2, Projectile.scale * (1/glowscale), SpriteEffects.None, 0);

			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Lerp(drawColor, Color.White, 0.25f) * 0.8f, Projectile.rotation, bloom.Size() / 2, Projectile.scale/3.5f, SpriteEffects.None, 0);
		}
	}
}