using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Enums;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.PirateStuff.DuelistLegacy
{
	public class DuelistLegacy : ModItem
	{
		public bool ChargeReady => charge % 3 == 2;

		private int charge;

		public override bool AltFunctionUse(Player player) => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Tooltip.SetDefault("Right click to fire a powerful shotgun blast \nCharges up a super attack after 2 swings\nRelease this special attack as either a slash or a blast");
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 36;
			item.height = 44;
			item.useTime = 12;
			item.useAnimation = 12;
			item.reuseDelay = 20;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 10f;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.crit = 4;
			item.rare = 5;
			item.shootSpeed = 1f;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<DuelistSlash>();
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				item.shoot = ModContent.ProjectileType<DuelistGun>();
			else
				item.shoot = ModContent.ProjectileType<DuelistSlash>();

			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 direction = new Vector2(speedX, speedY);
				if (ChargeReady)
				{
					for (int i = 0; i < 15; i++)
					{
						Dust dust = Dust.NewDustDirect(player.Center + (direction * 20), 0, 0, ModContent.DustType<DuelistBubble2>());
						dust.velocity = direction.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(2, 10);
					}
					player.GetModPlayer<MyPlayer>().Shake += 12;
					Projectile.NewProjectile(position + (direction * 20) + (direction.RotatedBy(-1.57f * player.direction) * 15), direction, ModContent.ProjectileType<DuelistBlastSpecial>(), damage * 2, knockBack * 1.5f, player.whoAmI);
				}
				else
				{
					for (int i = 0; i < 10; i++)
					{
						Dust dust = Dust.NewDustDirect(player.Center + (direction * 20), 0, 0, ModContent.DustType<DuelistSmoke>());
						dust.velocity = direction.RotatedBy(Main.rand.NextFloat(-0.4f,0.4f)) * Main.rand.NextFloat(5, 15);
						dust.scale = Main.rand.NextFloat(0.5f, 0.75f);
						dust.alpha = 40 + Main.rand.Next(40);
						dust.rotation = Main.rand.NextFloat(6.28f);
					}
					Projectile.NewProjectile(position + (direction * 20) + (direction.RotatedBy(-1.57f * player.direction) * 15), direction, ModContent.ProjectileType<DuelistBlast>(), damage, knockBack, player.whoAmI);
				}
				charge = 0;
			}
			else
			{
				Vector2 direction = new Vector2(speedX, speedY);
				Projectile proj = Projectile.NewProjectileDirect(position + (direction * 20) + (direction.RotatedBy(-1.57f * player.direction) * 20), Vector2.Zero, ModContent.ProjectileType<DuelistSlash>(), damage, knockBack, player.whoAmI);
				var mp = proj.modProjectile as DuelistSlash;
				mp.Phase = charge % 3;
				charge++;
				return false;
			}
			speedX = speedY = 0;
			return true;
		}
	}

	internal class DuelistSlash : ModProjectile
	{
		public const float SwingRadians = MathHelper.Pi * 0.75f; //Total radians of the sword's arc

		public int Phase;

		public int Timer;

		public Player Player => Main.player[projectile.owner];

		public bool Empowered => Phase == 2;

		Vector2 direction = Vector2.Zero;

		private bool initialized = false;

		private float rotation;

		private bool flip = false;

		public int MaxFrames
		{
			get
			{
				switch (Phase)
				{
					case 0:
						return 11;
					case 1:
						return 11;
					case 2:
						return 8;
					default:
						return 19;
				}
			}
		}

		public int SwingDirection
		{
			get
			{
				switch (Phase)
				{
					case 0:
						return -1 * Math.Sign(direction.X);
					case 1:
						return 1 * Math.Sign(direction.X);
					default:
						return 0;

				}
			}
		}

		public int SwingTime => MaxFrames * 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 1;//11, 11, 9, 19
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(150, 250);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
			projectile.ownerHitCheck = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineDirection = rotation.ToRotationVector2();
			float collisionPoint = 0;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Player.Center, Player.Center + (lineDirection * projectile.width), projectile.height, ref collisionPoint))
				return true;
			return false;
		}
		public override bool? CanCutTiles()
		{
			return true;
		}

		// Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
		public override void CutTiles()
		{
			Vector2 lineDirection = rotation.ToRotationVector2();
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(Player.Center, Player.Center + (lineDirection * projectile.width), projectile.height, DelegateMethods.CutTiles);
		}
		public override void AI()
		{
			projectile.velocity = Vector2.Zero;
			Player.itemTime = Player.itemAnimation = 5;
			Player.heldProj = projectile.whoAmI;

			if (!initialized)
			{
				initialized = true;
				direction = Player.DirectionTo(Main.MouseWorld);
				direction.Normalize();
				projectile.rotation = direction.ToRotation();
				switch (Phase)
				{
					case 0:
						projectile.Size = new Vector2(150, 50);
						break;
					case 1:
						projectile.Size = new Vector2(150, 50);
						flip = !flip;
						break;
					case 2:
						projectile.Size = new Vector2(150, 250);
						projectile.Center -= (direction * 150);
						break;
				}

				if (direction.X < 0)
					flip = !flip;

				if (Empowered)
				{
					projectile.damage *= 2;
					Player.GetModPlayer<MyPlayer>().Shake += 12;
				}
			}

			if (Empowered)
				projectile.Center = Player.Center - (direction * 50);
			else
				projectile.Center = Player.Center + (direction.RotatedBy(-1.57f) * 20);

			Timer++;
			float progress = Math.Min(Timer / (float)SwingTime, 1);
			progress = EaseFunction.EaseCircularInOut.Ease(progress);
			rotation = projectile.rotation + MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection, progress);

			Player.direction = Math.Sign(rotation.ToRotationVector2().X);

			Player.itemRotation = rotation;
			if (Player.direction != 1)
			{
				Player.itemRotation -= 3.14f;
			}
			Player.itemRotation = MathHelper.WrapAngle(Player.itemRotation);

			if (!Empowered)
			{
				if (projectile.frame > 2 && projectile.frame < 5)
				{
					/*StarParticle particle = new StarParticle(
						Player.Center + ((rotation - 0.4f).ToRotationVector2() * 95),
						Main.rand.NextVector2Circular(1, 1),
						Color.Cyan,
						Main.rand.NextFloat(0.1f, 0.2f),
						Main.rand.Next(20, 40));

					ParticleHandler.SpawnParticle(particle);*/

					for (int i = 0; i < 2; i++)
						Dust.NewDustPerfect(Player.Center + ((rotation - 0.4f).ToRotationVector2() * 95), ModContent.DustType<DuelistBubble>(), Main.rand.NextVector2Circular(1, 1));
				}
			}

			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 0)
				projectile.frame++;
			if (projectile.frame >= MaxFrames)
			{
				if (Phase == 1)
				{
					Projectile.NewProjectile(Player.Center, Vector2.Zero, ModContent.ProjectileType<DuelistActivation>(), 0, 0, Player.whoAmI);
				}
				projectile.active = false;
			}

			if (projectile.frame >= 2 && projectile.frame < 7)
				projectile.friendly = true;
			else
				projectile.friendly = false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Color.White;
			color.A = 120;
			color *= 0.8f;

			if (!Empowered)
			{
				Texture2D tex2 = Main.projectileTexture[projectile.type];
				if (flip)
					spriteBatch.Draw(tex2, Player.Center - Main.screenPosition, null, lightColor, rotation + 2.355f, new Vector2(tex2.Width, tex2.Height), projectile.scale, SpriteEffects.FlipHorizontally, 0f);
				else
					spriteBatch.Draw(tex2, Player.Center - Main.screenPosition, null, lightColor, rotation + 0.785f, new Vector2(0, tex2.Height), projectile.scale, SpriteEffects.None, 0f);
			}

			Texture2D tex;
			switch (Phase)
			{
				case 0:
					tex = ModContent.GetTexture(Texture + "One");
					break;
				case 1:
					tex = ModContent.GetTexture(Texture + "Two");
					break;
				case 2:
					tex = ModContent.GetTexture(Texture + "Special");
					break;
				default:
					tex = Main.projectileTexture[projectile.type];
					break;
			}
			int frameHeight = tex.Height / MaxFrames;
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			if (flip)
			{
				if (direction.X > 0)
					spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, color, projectile.rotation, new Vector2(0, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
				else
					spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, color, projectile.rotation + 3.14f, new Vector2(tex.Width, frameHeight / 2), projectile.scale, SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				if (direction.X > 0)
					spriteBatch.Draw(tex, projectile.Center - Main.screenPosition - (direction.RotatedBy(-1.57f) * 15), frame, color, projectile.rotation, new Vector2(0, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
				else
					spriteBatch.Draw(tex, projectile.Center - Main.screenPosition - (direction.RotatedBy(-1.57f) * 15), frame, color, projectile.rotation + 3.14f, new Vector2(tex.Width, frameHeight / 2), projectile.scale, SpriteEffects.FlipHorizontally, 0f);
			}
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitDirection = Math.Sign(direction.X);
		}
	}
	internal class DuelistGun : ModProjectile
	{
		public float Recoil = 0f;

		private Vector2 initialDirection = Vector2.Zero;

		private Vector2 CurrentDirection => initialDirection.RotatedBy(Recoil);

		private bool initialized = false;

		private Player Player => Main.player[projectile.owner];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(32, 32);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
			projectile.ownerHitCheck = true;
			projectile.timeLeft = 40;
		}

		public override void AI()
		{
			Player.itemTime = Player.itemAnimation = 5;
			Player.heldProj = projectile.whoAmI;
			projectile.Center = Player.Center;

			if (!initialized)
			{
				initialized = true;
				initialDirection = Player.DirectionTo(Main.MouseWorld);
				initialDirection.Normalize();
				Recoil = Player.direction * -0.75f;
			}

			Player.itemRotation = CurrentDirection.ToRotation();
			if (Player.direction != 1)
			{
				Player.itemRotation -= 3.14f;
			}
			Recoil *= 0.95f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];

			int height = texture.Height / Main.projFrames[projectile.type];
			int y2 = height * projectile.frame;
			Vector2 position = (player.Center + (CurrentDirection * 15)) - Main.screenPosition;

			if (player.direction == 1)
			{
				SpriteEffects effects1 = SpriteEffects.None;
				Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, CurrentDirection.ToRotation(), new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			else
			{
				SpriteEffects effects1 = SpriteEffects.FlipHorizontally;
				Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, CurrentDirection.ToRotation() - 3.14f, new Vector2((float)texture.Width / 2f, (float)height / 2f), projectile.scale, effects1, 0.0f);
			}
			return false;
		}
	}

	internal class DuelistBlast : ModProjectile
	{

		protected virtual Color color => Color.White;

		int direction = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(225, 75);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
		}
		public override void AI()
		{
			if (projectile.velocity != Vector2.Zero)
			{
				direction = Math.Sign(projectile.velocity.X);
				projectile.rotation = projectile.velocity.ToRotation();
			}
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 0)
				projectile.frame++;
			if (projectile.frame >= Main.projFrames[projectile.type])
				projectile.active = false;
			if (projectile.frame >= Main.projFrames[projectile.type] / 2)
				projectile.friendly = false;

			CreateParticles();
		}

		protected virtual void CreateParticles()
		{
			Vector2 lineDirection = projectile.rotation.ToRotationVector2() * (projectile.width * 0.7f);
			Vector2 lineOffshoot = (projectile.rotation + 1.57f).ToRotationVector2() * projectile.height * 0.3f;
			for (int i = 0; i < 3; i++)
			{
				Vector2 position = projectile.Center + (lineDirection * Main.rand.NextFloat()) + (lineOffshoot * Main.rand.NextFloat(-1f, 1f));
				Dust.NewDustPerfect(position, 6, Main.rand.NextVector2Circular(1, 1) + ((projectile.rotation + Main.rand.NextFloat(-0.35f,0.35f)).ToRotationVector2() * 5), 0, default, 1.3f).noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			if (direction == 1)
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color, projectile.rotation, new Vector2(0, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			else
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color, projectile.rotation + 3.14f, new Vector2(tex.Width, frameHeight / 2), projectile.scale, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineDirection = projectile.rotation.ToRotationVector2();
			float collisionPoint = 0;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + (lineDirection * projectile.width), projectile.height, ref collisionPoint))
				return true;
			return false;
		}


		public override bool? CanCutTiles()
		{
			return true;
		}

		// Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
		public override void CutTiles()
		{
			Vector2 lineDirection = projectile.rotation.ToRotationVector2();
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(projectile.Center, projectile.Center + (lineDirection * projectile.width), projectile.height, DelegateMethods.CutTiles);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitDirection = direction;
		}
	}

	internal class DuelistBlastSpecial : DuelistBlast
	{
		protected override Color color => new Color(255, 255, 255, 120) * 0.8f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 11;
		}
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.Size = new Vector2(300, 100);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
		}

		protected override void CreateParticles()
		{
			Vector2 lineDirection = projectile.rotation.ToRotationVector2() * (projectile.width * 0.7f);
			Vector2 lineOffshoot = (projectile.rotation + 1.57f).ToRotationVector2() * projectile.height * 0.3f;
			if (projectile.frame < 7)
			{
				Vector2 position = projectile.Center + (lineDirection * Main.rand.NextFloat()) + (lineOffshoot * Main.rand.NextFloat(-1f, 1f));
				Dust.NewDustPerfect(position, ModContent.DustType<DuelistBubble>(), Main.rand.NextVector2Circular(1, 1) + (projectile.rotation.ToRotationVector2() * 2));
			}
		}
	}

	internal class DuelistActivation : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 19;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(225, 75);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
			projectile.ownerHitCheck = true;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HeldItem.modItem is DuelistLegacy modItem && !modItem.ChargeReady)
				projectile.active = false;

			Vector2 direction = player.DirectionTo(Main.MouseWorld);
			direction.Normalize();
			projectile.rotation = direction.ToRotation();

			projectile.Center = player.Center + (direction * 15);
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 0)
				projectile.frame++;
			if (projectile.frame >= Main.projFrames[projectile.type])
				projectile.active = false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Color.White;
			color.A = 120;
			color *= 0.8f;

			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color, projectile.rotation, new Vector2(0, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}