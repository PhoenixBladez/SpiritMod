/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Weapon.Summon.WyvernStaff
{
	public class WyvernStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Staff");
			Tooltip.SetDefault("Hold down and release to summon a wyvern");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 320;
			item.useAnimation = 320;
			item.width = 40;
			item.height = 32;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Summon;
			item.channel = true;
			item.noMelee = true;
			item.shootSpeed = 8f;
			item.knockBack = 5f;
			item.damage = 36;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<WyvernStaffProj>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
	}

	public class WyvernStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Staff");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			projectile.width = 90;
			projectile.height = 90;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.ownerHitCheck = true;
			projectile.aiStyle = -1;
		}
		readonly int height = 54;
		readonly int width = 50;

		double radians = 0;
		float alphaCounter = 0;
		bool released = false;

		Projectile head;
		int latestSegment;
		int numSegments = 0;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			alphaCounter += 0.08f;

			if (numSegments == 0)
			{
				numSegments++;
				int headint = Projectile.NewProjectile(player.Center - new Vector2(0, 150), Vector2.Zero, ModContent.ProjectileType<WyvernStaffHead>(), projectile.damage, Projectile.knockBack, projectile.owner);
				head = Main.projectile[headint];
				latestSegment = headint;
			}

			projectile.ai[0]++;
			if ((int)projectile.ai[0] % 15 == 0 && numSegments < 25)
			{
				Main.projectile[latestSegment].frame = 1 + (int)projectile.ai[0] % 2;
				latestSegment = Projectile.NewProjectile(Main.projectile[latestSegment].Center, Vector2.Zero, ModContent.ProjectileType<WyvernStaffBody>(), projectile.damage, Projectile.knockBack, projectile.owner, latestSegment);
				numSegments++;

				if (!Main.projectile[latestSegment].active)
					Main.NewText("Uh oh. Something went wrong. Report to Spirit Mod devs immediately: Wyvern Staff Bad Segmentation");
			}

			head.Center = player.Center + ((float)radians + 3.14f).ToRotationVector2() * 80;

			if (player.direction == 1)
			{
				head.spriteDirection = -1;
				head.rotation = (float)radians;
			}
			else
			{
				head.spriteDirection = -1;
				head.rotation = (float)radians + MathHelper.Pi;
			}

			if (!released)
				projectile.scale = MathHelper.Clamp(projectile.ai[0] / 10f, 0, 1);

			if (player.direction == 1)
				radians += 30f / 200;
			else
				radians -= 30f / 200;

			if (radians > 6.28)
				radians -= 6.28;

			if (radians < -6.28)
				radians += 6.28;

			projectile.velocity = Vector2.Zero;

			Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.position);
			double throwingAngle = direction.ToRotation() + 3.14;
			projectile.position = player.Center - (Vector2.UnitX.RotatedBy(radians) * 40) - (projectile.Size / 2);
			player.itemAnimation -= 14;

			while (player.itemAnimation < 3)
			{
				Main.PlaySound(SoundID.Item1, projectile.Center);
				player.itemAnimation += 320;
			}

			player.itemTime = player.itemAnimation;

			if (player.whoAmI == Main.myPlayer)
				player.ChangeDir(Math.Sign(direction.X));

			if (player.direction != 1)
				throwingAngle -= 6.28;

			if ((!player.channel && Math.Abs(radians - throwingAngle) < 1 && projectile.ai[0] > 20) || released)
			{
				released = true;
				projectile.scale -= 0.35f;
				if (projectile.scale < 0.35f)
				{
					player.itemTime = player.itemAnimation = 2;
					projectile.active = false;
					head.velocity = direction * 20;
					if (head.modProjectile is WyvernStaffHead modProj)
					{
						modProj.attack = true;
						modProj.deathCounter = 30 * numSegments;
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor;
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}*/