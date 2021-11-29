using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Terraria.Graphics.Shaders;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.OlympiumSet.ArtemisHunt
{
	public class ArtemisHunt : ModItem
	{

		public override bool AltFunctionUse(Player player) => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis's Hunt");
			Tooltip.SetDefault("Hit enemies to mark them \n Right click to fire a volley of arrows at marked foes");
		}

		public override void SetDefaults()
		{
			item.damage = 43;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.UseSound = SoundID.Item5;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2);
			item.autoReuse = true;
			item.shootSpeed = 12f;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(new Vector2(item.Center.X, item.Center.Y), 0.075f, 0.255f, 0.193f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/OlympiumSet/ArtemisHunt/ArtemisHunt_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White * .6f,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
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
	}
	public class ArtemisHuntArrow : ModProjectile, ITrailProjectile, IDrawAdditive
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
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * .75f, 0.255f * .75f, 0.193f * .75f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.AddBuff(ModContent.BuffType<ArtemisMark>(), 180);
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245), new Color(48, 255, 176)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 50f, new DefaultShader());
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 12f;
			for (int num257 = 0; num257 < 18; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.PoisonStaff, 0f, 0f, 0, Color.White, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].shader = GameShaders.Armor.GetSecondaryShader(22, Main.LocalPlayer);
				vector9 -= value19 * 8f;
			}
			DustHelper.DrawDustImage(projectile.Center, 89, 0.125f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(77, 255, 193) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Sets/OlympiumSet/ArtemisHunt/ArtemisHunt_Glow");
				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			{
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * .6f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f); ;
				}
			}
			return false;
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
					offsetAngle += SPREAD;
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, direction.RotatedBy(offsetAngle) * 20, ModContent.ProjectileType<ArtemisHuntVolley>(), projectile.damage, projectile.knockBack, projectile.owner);

					ImpactLine line = new ImpactLine(projectile.Center - (direction.RotatedBy(offsetAngle) * 50), direction.RotatedBy(offsetAngle) * 4, Color.Gold, new Vector2(0.25f, 2f), 70);
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
			if (Main.rand.Next(7) == 1)
			{
				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(5, 5),
				projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f,0.3f)) * 0.1f,
				Main.rand.NextBool() ? Color.Gold : new Color(48, 255, 176),
				Main.rand.NextFloat(0.05f,0.15f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}
			Lighting.AddLight(projectile.Center, new Color(48, 255, 176).ToVector3() * 0.3f);
			return true;
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(48, 255, 176), new Color(0, 168, 255)), new RoundCap(), new DefaultTrailPosition(), 8f, 400f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
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
				ImpactLine line = new ImpactLine(target.Center - (vel * 10), vel, Main.rand.NextBool() ? Color.Gold : new Color(48, 255, 176), new Vector2(0.25f, Main.rand.NextFloat(0.5f,1.5f)), 70);
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
	}
}