using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Bullet;
using Terraria.ID;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System.Collections.Generic;


namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenFleetProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			DisplayName.SetDefault("Heaven Fleet");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.timeLeft = 999999;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
		}

		int maxCounter = 1;
		bool firing = false;
		Vector2 direction = Vector2.Zero;

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

			player.itemTime = 10; // Set item time to 10 frames while we are used
			player.itemAnimation = 10; // Set item animation time to 10 frames while we are used
			projectile.position = player.Center;

			direction = Vector2.Normalize(Main.MouseWorld - (player.Center - new Vector2(4, 4))) * 10f;
			Vector2 dustUnit = (direction * 2.5f).RotatedBy(Main.rand.NextFloat(-1, 1)) * 0.03f;
			Vector2 pulseUnit = (direction * 2.5f) * 0.03f;

			Vector2 dustOffset = player.Center + (direction * (5f + 3 * (float)Math.Sqrt(projectile.localAI[0] / 100f))) + player.velocity;
			Color color = Color.Lerp(new Color(35, 57, 222), new Color(140, 238, 255), (float)Math.Sqrt(projectile.localAI[0] / 100f));
			Vector2 spawnPos = dustOffset + (pulseUnit * 30);


			if (player.channel && !firing)
			{
				if (projectile.localAI[0] < 100)
				{
					maxCounter++;
					projectile.localAI[0]++;

					var dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), 226);
					dust.velocity = Vector2.Zero - (dustUnit * 4);
					dust.noGravity = true;
					dust.scale = (float)Math.Sqrt(projectile.localAI[0] / 100f);
					if (projectile.localAI[0] % 10 == 9)
					{
						ParticleHandler.SpawnParticle(new PulseCircle(spawnPos, color * 0.4f, (.5f + .8f * (float)Math.Sqrt(projectile.localAI[0] / 100f)) * 100, 20, PulseCircle.MovementType.InwardsQuadratic)
						{
							Angle = player.itemRotation,
							ZRotation = 0.6f,
							RingColor = color,
							Velocity = Vector2.Zero - (pulseUnit * 1.5f)
						});
						Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge1").WithPitchVariance(0.1f).WithVolume(0.275f), player.Center);
					}

				}
				direction = direction.RotatedBy(Main.rand.NextFloat(0 - ((float)Math.Sqrt(projectile.localAI[0]) / 300f), ((float)Math.Sqrt(projectile.localAI[0]) / 300f)));
				player.itemRotation = direction.ToRotation();

				if (player.direction != 1)
					player.itemRotation -= 3.14f;
			}
			else
			{
				firing = true;

				if (projectile.localAI[0] > 0)
				{
					Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyBlastMedium").WithPitchVariance(0.1f).WithVolume(0.275f), player.Center);
					ParticleHandler.SpawnParticle(new PulseCircle(dustOffset + (pulseUnit * 10), color * 0.4f, (.5f + .8f * (float)Math.Sqrt(projectile.localAI[0] / 100f)) * 100, 20, PulseCircle.MovementType.OutwardsQuadratic)
					{
						Angle = player.itemRotation,
						ZRotation = 0.6f,
						RingColor = color,
						Velocity = Vector2.Zero - (pulseUnit * 1.5f)
					});
				}
				while (projectile.localAI[0] >= 0)
				{
					if (projectile.localAI[0] > 90)
					{
						for (int i = 0; i < 20; i++)
						{
							var dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), 226);
							dust.velocity = Vector2.Zero + (Vector2.Normalize(Main.MouseWorld - (player.Center - new Vector2(4, 4))).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(2f, 6f) * 3f);
							dust.noGravity = true;
							dust.scale = (float)Math.Sqrt(projectile.localAI[0] / 100f);
						}
					}
					projectile.localAI[0] -= 10;
					Vector2 toShoot = direction.RotatedBy(Main.rand.NextFloat(0 - ((float)Math.Sqrt(200 - maxCounter) / 40f), (float)Math.Sqrt(200 - maxCounter) / 40f));
					player.itemRotation = (toShoot.ToRotation() + direction.ToRotation()) / 2;
					if (player.direction != 1)
						player.itemRotation -= 3.14f;
					projectile.velocity = toShoot;
					toShoot *= 2.3f;
					toShoot *= (float)Math.Pow(maxCounter, 0.18);
					player.GetModPlayer<MyPlayer>().Shake += 1;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectileDirect(player.Center + (direction * 4), toShoot * new Vector2(Main.rand.NextFloat(.6f, 1.3f), Main.rand.NextFloat(0.6f, 1.2f)), ModContent.ProjectileType<HeavenfleetStar>(), projectile.damage, projectile.knockBack, projectile.owner);
				}
				projectile.active = false;
			}
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			direction = Vector2.Normalize(Main.MouseWorld - (player.Center - new Vector2(4, 4))) * 10f;
			Vector2 Offset = player.Center + direction * 3f + player.velocity;
			if (projectile.localAI[0] == 100)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + (0.225f * k);
					if (num107 < 0)
					{
						num107 = 0;
					}
					Color color = Color.White * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

					float scale = projectile.scale;
					Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Sets/GunsMisc/HeavenFleet/HeavenFleet_Glow");

					spriteBatch.Draw(tex, Offset - Main.screenPosition, null, color, player.itemRotation, tex.Size() / 2, num107, default, default);
				}
			}
			return true;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Player player = Main.player[projectile.owner];
			direction = Vector2.Normalize(Main.MouseWorld - (player.Center - new Vector2(4, 4))) * 10f;
			Vector2 Offset = player.Center + direction * 2.5f + player.velocity;
			if (projectile.localAI[0] == 100)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{

					Color color = Color.White * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

					float scale = projectile.scale;
					Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Sets/GunsMisc/HeavenFleet/HeavenFleet_Lights");

					spriteBatch.Draw(tex, Offset - Main.screenPosition, null, color, player.itemRotation, tex.Size() / 2, 1f, default, default);
				}
			}
		}
	}
}