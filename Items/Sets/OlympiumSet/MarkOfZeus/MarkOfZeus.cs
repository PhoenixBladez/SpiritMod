using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.OlympiumSet.MarkOfZeus
{
	public class MarkOfZeus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage\nConsumes 20 mana per second while charging");
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.noMelee = true;
			Item.channel = true;
			Item.rare = ItemRarityID.LightRed;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 15;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 8;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<MarkOfZeusProj>();
			Item.shootSpeed = 0f;
			Item.value = Item.sellPrice(0, 2, 0, 0);
		}

		public override bool CanUseItem(Player player) => player.statMana > 20;
	}

	public class MarkOfZeusProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mark of Zeus");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.alpha = 0;
			Projectile.timeLeft = 999999;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		float counter = 3.15f;
		int manaCounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);
		bool charged = false;
		bool firing = false;
		float growCounter;
		float glowCounter;

		public override bool PreAI()
		{
			growCounter++;

			if (growCounter == 10)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/ElectricCharge") with { PitchVariance = 0.4f, Volume = 0.2f }, Projectile.Center);

			Player player = Main.player[Projectile.owner];

			if (player.statMana <= 0)
				Projectile.Kill();

			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 direction2 = Main.MouseWorld - (Projectile.position);
				direction2.Normalize();
				direction2 *= counter;
				Projectile.ai[0] = direction2.X;
				Projectile.ai[1] = direction2.Y;
				Projectile.netUpdate = true;
			}

			Vector2 direction = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			if (player.channel && !firing)
			{
				Projectile.position = player.position + holdOffset;
				if (Main.rand.NextBool(4))
					Dust.NewDustDirect(Projectile.Center + ((Projectile.rotation + 1.57f).ToRotationVector2() * Main.rand.Next(-30, 30)), 2, 2, DustID.Firework_Yellow).noGravity = true;

				if (Main.rand.NextBool(8))
				{
					int timeLeft = Main.rand.Next(20, 50);
					var particle = new StarParticle(Projectile.Center + ((Projectile.rotation + 1.57f).ToRotationVector2() * Main.rand.Next(-30, 30)), Main.rand.NextVector2Circular(1.5f, 1.5f),
						Color.Gold, Main.rand.NextFloat(0.1f, 0.2f),
					timeLeft);
					ParticleHandler.SpawnParticle(particle);
				}

				if (counter < 15)
				{
					counter += 0.30f;
					manaCounter++;
					if (manaCounter % 7 == 0)
					{
						if (player.statMana > 0)
						{
							player.statMana -= 5;
							player.manaRegenDelay = 60;
							if (player.statMana <= 0)
								Launch(player, direction);
						}
						else
							firing = true;
					}
				}
				else if (!charged)
				{
					charged = true;
					SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
				}
				else
					glowCounter += 0.025f;

				Projectile.rotation = direction.ToRotation() - 1.57f;

				if (direction.X > 0)
				{
					holdOffset.X = -10;
					player.direction = 1;
				}
				else
				{
					holdOffset.X = 10;
					player.direction = 0;
				}
			}
			else
			{
				Launch(player, direction);
				Projectile.active = false;
			}
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			return true;
		}

		int flickerTime = 0;

		private void Launch(Player player, Vector2 direction)
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Thunder") with { PitchVariance = 0.6f, Volume = 0.6f }, Projectile.Center);

			if (Projectile.owner == Main.myPlayer)
			{
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, direction, ModContent.ProjectileType<MarkOfZeusProj2>(), (int)(Projectile.damage * Math.Sqrt(counter) * 0.5f), Projectile.knockBack, Projectile.owner);
				if (Main.projectile[proj].ModProjectile is MarkOfZeusProj2 modItem)
					modItem.charge = counter;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float growScale = growCounter > 10 ? 1 : (growCounter / 10f);
			Player player = Main.player[Projectile.owner];
			var effects = player.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

			lightColor = Color.White;

			if (charged)
			{
				float progress = glowCounter % 1;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, 0, tex.Width, tex.Height / 2),
					lightColor * transparency, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), Projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale * scale, effects, 0);
			}

			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, 0, tex.Width, tex.Height / 2),
				lightColor, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), Projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);

			if (counter >= 15 && flickerTime < 16)
			{
				flickerTime++;
				float flickerTime2 = flickerTime / 20f;
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0)
					alpha = 0;

				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, tex.Height / 2, tex.Width, tex.Height / 2), 
					Color.White * alpha, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), Projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);
			}
			return false;
		}
	}

	public class MarkOfZeusProj2 : ModProjectile
	{
		public MarkOfZeusPrimTrailTwo trail;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public float charge;
		private bool initialized = false;

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 36;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 5;
			Projectile.light = 0;
			AIType = ProjectileID.ThrowingKnife;
			Projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.timeLeft = 0;

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				if (Main.netMode != NetmodeID.Server)
				{
					trail = new MarkOfZeusPrimTrailTwo(Projectile, 2f * (float)(Math.Sqrt(charge) / 5));
					SpiritMod.primitives.CreateTrail(trail);
				}
			}

			trail?.AddPoints();

			if (Main.rand.NextBool(3))
			{
				int timeLeft = Main.rand.Next(40, 100);
				StarParticle particle = new StarParticle(
				Projectile.Center,
				Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1.57f, 1.57f)) * 0.3f,
				Color.Gold,
				Main.rand.NextFloat(0.1f, 0.2f),
				timeLeft);
				particle.TimeActive = (uint)(timeLeft / 2);
				ParticleHandler.SpawnParticle(particle);
			}
		}

		public override void Kill(int timeLeft)
		{
			SpiritMod.tremorTime = (int)(charge * 0.66f);
			SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);

			for (double i = 0; i < 6.28; i += Main.rand.NextFloat(1f, 2f))
			{
				int lightningproj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center + Projectile.velocity - (new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 5f), new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 2.5f, ModContent.ProjectileType<MarkOfZeusProj3>(), Projectile.damage, Projectile.knockBack, Projectile.owner, charge);
				Main.projectile[lightningproj].timeLeft = (int)(30 * Math.Sqrt(charge));
			}

			for (double i = 0; i < 6.28; i += 0.15)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(3f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(4f) * (float)(Math.Sqrt(charge) / 3));
				Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(1.8f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(2.4f) * (float)(Math.Sqrt(charge) / 3));
				dust.noGravity = true;
				dust2.noGravity = true;
				dust.scale = 0.75f;
				dust2.scale = 0.75f;
			}

			for (int j = 0; j < 13; j++)
			{
				var particle = new StarParticle(Projectile.Center + Main.rand.NextVector2Circular(10, 10) + Projectile.velocity, Main.rand.NextVector2Circular(7, 5), 
					Color.Gold, Main.rand.NextFloat(0.1f, 0.2f), Main.rand.Next(30, 60));
				ParticleHandler.SpawnParticle(particle);
			}
		}
	}

	public class MarkOfZeusProj3 : ModProjectile
	{
		public MarkOfZeusPrimTrail trail;

		private bool initialized = false;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mark of Zeus");

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.damage = 0;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 12;
		}

		Vector2 initialVelocity = Vector2.Zero;
		public Vector2 DrawPos;
		public int boost;

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				if (Main.netMode != NetmodeID.Server)
				{
					trail = new MarkOfZeusPrimTrail(Projectile, 2f * (float)(Math.Sqrt(Projectile.ai[0]) / 3));
					SpiritMod.primitives.CreateTrail(trail);
				}
			}

			trail?.AddPoints();

			if (initialVelocity == Vector2.Zero)
				initialVelocity = Projectile.velocity;

			if (Projectile.timeLeft % 10 == 0)
				Projectile.velocity = initialVelocity.RotatedBy(Main.rand.NextFloat(-1, 1));

			DrawPos = Projectile.position;
		}
	}
}