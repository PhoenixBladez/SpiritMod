using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Prim;
using SpiritMod.Particles;
using System;
using System.Linq;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.OlympiumSet.ArtemisHunt
{
	public class ArtemisHunt : ModItem
	{

		public override bool AltFunctionUse(Player player) => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iokheira");
			Tooltip.SetDefault("Hit enemies to mark them \nRight click to fire a volley of arrows at marked foes");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 43;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2);
			item.autoReuse = true;
			item.shootSpeed = 12f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<ArtemisHuntProj>(), damage, knockBack, player.whoAmI);
				return false;
			}
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 5).WithPitchVariance(0.2f), player.Center);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 20).WithPitchVariance(0.2f).WithVolume(.5f), player.Center);
				type = ModContent.ProjectileType<ArtemisHuntArrow>();
			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				item.noUseGraphic = true;
			else
				item.noUseGraphic = false;
			return base.CanUseItem(player);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_Glow"), rotation, scale);
	}
	public class ArtemisHuntArrow : ModProjectile
	{
		float noiseRotation;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 600;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void AI()
		{
			if (noiseRotation < 0.02f)
				noiseRotation = Main.rand.NextFloat(6.28f);
			noiseRotation += 0.02f;

			if (Main.rand.Next(5) == 1)
			{
				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(5, 5),
				projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.1f,
				new Color(48, 255, 176),
				Main.rand.NextFloat(0.08f, 0.23f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}

			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * .75f, 0.255f * .75f, 0.193f * .75f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = new Color(48, 255, 176);
			color.A = 0;

			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(7, 0), 1, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			SpiritMod.ConicalNoise.Parameters["vnoise"].SetValue(mod.GetTexture("Textures/voronoiLooping"));
			SpiritMod.ConicalNoise.Parameters["rotation"].SetValue(noiseRotation);
			SpiritMod.ConicalNoise.Parameters["transparency"].SetValue(0.8f);
			SpiritMod.ConicalNoise.Parameters["color"].SetValue(color.ToVector4());
			SpiritMod.ConicalNoise.CurrentTechnique.Passes[0].Apply();

			Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), projectile.Center + (projectile.rotation.ToRotationVector2() * 17)- Main.screenPosition, null, color, projectile.rotation - 1.57f, new Vector2(100, 83), new Vector2(0.6f, 0.4f), SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 8; i++)
			{
				Vector2 vel = Vector2.Zero - projectile.velocity;
				vel.Normalize();
				vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
				vel *= Main.rand.NextFloat(2, 5);
				ImpactLine line = new ImpactLine(target.Center - (vel * 10), vel, new Color(48, 255, 176), new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f)), 70);
				line.TimeActive = 30;
				ParticleHandler.SpawnParticle(line);

			}

			for (int j = 0; j < 7; j++)
			{
				Vector2 vel = Vector2.Zero - projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.25f;
				int timeLeft = Main.rand.Next(40, 100);

				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(10, 10) - (vel * 5),
				vel + Main.rand.NextVector2Circular(3, 3),
				new Color(48, 255, 176),
				Main.rand.NextFloat(0.1f, 0.2f),
				timeLeft);
				particle.TimeActive = (uint)(timeLeft / 2);
				ParticleHandler.SpawnParticle(particle);
			}

			if (!target.GetGlobalNPC<ArtemisGNPC>().artemisMarked)
				Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<ArtemisCrescent>(), 0, 0, projectile.owner, target.whoAmI);
			target.GetGlobalNPC<ArtemisGNPC>().artemisMarked = true;
			target.GetGlobalNPC<ArtemisGNPC>().artemisTicker = 180;
		}
	}
	public class ArtemisHuntProj : ModProjectile
	{
		const int FRAMETIME = 5;
		const float SPREAD = 0.1f;
		const int NUMBEROFSHOTS = 7;
		const int NUMBEROFFRAMES = 7;
		const int SHOOTTIME = 2;

		bool Firing => frame > 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis Hunt");
			Main.projFrames[projectile.type] = 8;
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
		}

		int frame = 0;
		int frameCounter = 0;

		float offsetAngle = -0.45f;

		int shootCounter = 0;

		int shots = NUMBEROFSHOTS;

		Vector2 direction = Vector2.Zero;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

			player.itemTime = 5; // Set item time to 5 frames while we are used
			player.itemAnimation = 5; // Set item animation time to 5 frames while we are used
			projectile.Center = player.Center;
			player.heldProj = projectile.whoAmI;

			direction = Main.MouseWorld - player.Center;
			direction.Normalize();

			if (frame < NUMBEROFFRAMES)
			{
				frameCounter++;
				if (frameCounter >= FRAMETIME)
				{
					frameCounter = 0;
					frame++;
					projectile.frame++;
				}
			}

			if (Firing)
			{
				shootCounter++;
				if (shootCounter % SHOOTTIME == 0 && projectile.timeLeft > 30)
				{
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 20).WithPitchVariance(0.2f), projectile.Center);

					offsetAngle += SPREAD;
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, direction.RotatedBy(offsetAngle) * 20, ModContent.ProjectileType<ArtemisHuntVolley>(), projectile.damage, projectile.knockBack, projectile.owner);

					ImpactLine line = new ImpactLine(projectile.Center - (direction.RotatedBy(offsetAngle) * 50), direction.RotatedBy(offsetAngle) * 4, new Color(125, 255, 253), new Vector2(0.25f, 2f), 70);
					line.TimeActive = 30; 
					ParticleHandler.SpawnParticle(line);

					shots--;
					if (shots <= 0)
						projectile.timeLeft = 30;
				}
				direction = direction.RotatedBy(offsetAngle);

			}
			else
			{

			}

			player.itemRotation = direction.ToRotation();
			if (player.direction != 1)
			{
				player.itemRotation -= 3.14f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D glow = mod.GetTexture("Items/Sets/OlympiumSet/ArtemisHunt/ArtemisHuntProj_Glow");

			int height = texture.Height / Main.projFrames[projectile.type];
			int y2 = height * projectile.frame;
			Vector2 position = (player.Center + (direction * -4)) - Main.screenPosition;

			if (player.direction == 1)
			{
				SpriteEffects effects1 = SpriteEffects.None;
				Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, direction.ToRotation(), new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(glow, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation(), new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			else 
			{
				SpriteEffects effects1 = SpriteEffects.FlipHorizontally;
				Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, direction.ToRotation() - 3.14f, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(glow, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation() - 3.14f, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			return false;
		}
	}
	public class ArtemisHuntVolley : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 600;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreAI()
		{
			if (Main.rand.Next(3) == 1)
			{
				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(5, 5),
				projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f,0.3f)) * 0.1f,
				Main.rand.NextBool() ? new Color(125, 255, 253) : new Color(48, 255, 176),
				Main.rand.NextFloat(0.05f,0.15f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}

			var target = Main.npc.Where(n => n.active && Vector2.Distance(n.Center, projectile.Center) < 200 && n.GetGlobalNPC<ArtemisGNPC>().artemisMarked).OrderBy(n => Vector2.Distance(n.Center, projectile.Center)).FirstOrDefault();
			if (target != default)
			{
				Vector2 direction = target.Center - projectile.Center;
				direction.Normalize();
				direction *= 10;
				projectile.velocity = Vector2.Lerp(projectile.velocity, direction, 0.05f);
			}
			Lighting.AddLight(projectile.Center, new Color(48, 255, 176).ToVector3() * 0.3f);
			return true;
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(48, 255, 176), new Color(125, 255, 253)), new RoundCap(), new DefaultTrailPosition(), 8f, 400f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(48, 255, 176) * .5f, new Color(255, 255, 255) * 0.3f), new RoundCap(), new DefaultTrailPosition(), 26f, 100f, new DefaultShader());
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 6; i++)
			{
				Vector2 vel = Vector2.Zero - projectile.velocity;
				vel.Normalize();
				vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
				vel *= Main.rand.NextFloat(2, 5);
				ImpactLine line = new ImpactLine(target.Center - (vel * 10), vel, Main.rand.NextBool() ? new Color(125, 255, 253) : new Color(48, 255, 176), new Vector2(0.25f, Main.rand.NextFloat(0.5f,1.5f)), 70);
				line.TimeActive = 30;
				ParticleHandler.SpawnParticle(line);

			}

			for (int j = 0; j < 10; j++)
			{
				Vector2 vel = Vector2.Zero - projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.25f;
				int timeLeft = Main.rand.Next(40, 100);

				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(10, 10) - (vel * 5),
				vel + Main.rand.NextVector2Circular(3, 3),
				new Color(48, 255, 176),
				Main.rand.NextFloat(0.1f, 0.2f),
				timeLeft);
				particle.TimeActive = (uint)(timeLeft / 2); 
				ParticleHandler.SpawnParticle(particle);
			}
			StarParticle particle2 = new StarParticle(
			projectile.Center,
			Vector2.Zero,
			Color.White,
			0.6f,
			20);
			ParticleHandler.SpawnParticle(particle2);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (target.GetGlobalNPC<ArtemisGNPC>().artemisMarked)
				damage = (int)(damage * 1.5f);
		}
	}

	public class ArtemisCrescent : ModProjectile
	{
		private NPC target => Main.npc[(int)projectile.ai[0]];

		private float counter;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis Crescent");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.timeLeft = 12;
			projectile.ignoreWater = true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, new Color(48, 255, 176).ToVector3() * 0.3f);
			counter += 0.025f;
			if (target.active)
			{
				if (target.GetGlobalNPC<ArtemisGNPC>().artemisMarked)
				{
					projectile.scale = MathHelper.Clamp(counter * 3, 0, 1);
					projectile.timeLeft = 12;
				}
				else
					projectile.scale -= 0.083f;
				projectile.Center = target.Center;
			}
			else
				projectile.active = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float progress = counter % 1;
			float transparency = (float)Math.Pow(1 - progress, 2);
			float scale = 1 + progress;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * transparency, projectile.rotation, tex.Size() / 2, scale * projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
	}
	public class ArtemisGNPC : GlobalNPC
	{
		public bool artemisMarked;

		public int artemisTicker;

		public override bool InstancePerEntity => true;

		public override void PostAI(NPC npc)
		{
			if(artemisMarked) artemisTicker--;

			if (artemisTicker <= 0) artemisMarked = false;
			else artemisMarked = true;
		}
	}
}