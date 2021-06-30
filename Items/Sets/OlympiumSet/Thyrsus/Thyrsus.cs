using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.OlympiumSet.Thyrsus
{

	//TODO: Make it so you cant throw one if theres one active
	//TODO: Make it so right click destroys the currently alive one
	public class Thyrsus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			Tooltip.SetDefault("Throw at a surface to grow damaging vines");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.noMelee = true;
			item.rare = ItemRarityID.LightRed;
			item.width = 18;
			item.height = 18;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<ThyrsusProj>();
			item.shootSpeed = 10f;
			item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
	public class ThyrsusProj : ModProjectile
	{
		bool stuck = false;
		float shrinkCounter = 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			aiType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (stuck)
			{
				projectile.velocity = Vector2.Zero;
				if (projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (projectile.scale < 0.3f)
					{
						projectile.active = false;
					}
					if (projectile.scale > 1)
						projectile.scale = ((projectile.scale - 1) / 2f) + 1;
				}
				return false;
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//TODO: Playsound here
			if (!stuck)
			{
				stuck = true;
				projectile.tileCollide = false;
				projectile.timeLeft = 375;
				Vector2 velocity = new Vector2(oldVelocity.X == projectile.velocity.X ? 0 : Math.Sign(oldVelocity.X), oldVelocity.Y == projectile.velocity.Y ? 0 : Math.Sign(oldVelocity.Y));
				//Main.NewText("X: " + velocity.X.ToString());
				//Main.NewText("Y: " + velocity.Y.ToString());
				Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, ModContent.ProjectileType<ThyrsusProjTwo>(), projectile.damage, projectile.knockBack, projectile.owner);
				if (proj.modProjectile is ThyrsusProjTwo modproj)
				{
					velocity = velocity.RotatedBy(-1.57f / 2);
					velocity.Normalize();
					modproj.moveDirection = velocity;
				}
				proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, ModContent.ProjectileType<ThyrsusProjTwo>(), projectile.damage, projectile.knockBack, projectile.owner);
				if (proj.modProjectile is ThyrsusProjTwo modproj2)
				{
					velocity = velocity.RotatedBy(1.57f);
					velocity.Normalize();
					modproj2.moveDirection = velocity;
				}
				projectile.velocity = Vector2.Zero;
			}
			return false;
		}
	}
	public class ThyrsusProjTwo : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vine");


		public ThyrsusPrimTrail trail;
		public List<Vector2> points = new List<Vector2>();

		public Vector2 moveDirection;
		public Vector2 newVelocity = Vector2.Zero;
		public float speed = 3f;

		private float growCounter = 0;
		private bool primsCreated;
		bool collideX = false;
		bool collideY = false;
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 0;
			projectile.width = projectile.height = 8;
			projectile.timeLeft = 750;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			if (!primsCreated)
			{
				trail = new ThyrsusPrimTrail(projectile);

				SpiritMod.primitives.CreateTrail(trail);
				primsCreated = true;
			}

			if (growCounter < 1)
				projectile.scale = growCounter += 0.1f;

			newVelocity = Collide();
			if (Math.Abs(newVelocity.X) < 0.5f)
				collideX = true;
			else
				collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				collideY = true;
			else
				collideY = false;

			if (projectile.ai[1] == 0f)
			{
				projectile.rotation += (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideY)
				{
					projectile.ai[0] = 2f;
				}
				if (!collideY && projectile.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					projectile.ai[1] = 1f;
					projectile.ai[0] = 1f;
				}
				if (collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					projectile.ai[1] = 1f;
				}
			}
			else
			{
				projectile.rotation -= (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideX)
				{
					projectile.ai[0] = 2f;
				}
				if (!collideX && projectile.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					projectile.ai[1] = 0f;
					projectile.ai[0] = 1f;
				}
				if (collideY)
				{
					moveDirection.X = -moveDirection.X;
					projectile.ai[1] = 0f;
				}
			}
			if (projectile.timeLeft > 600)
			{
				projectile.velocity = speed * moveDirection;
				projectile.velocity = Collide();
				trail._addPoints = true;
			}
			else
			{
				projectile.velocity = Vector2.Zero;
				trail._addPoints = false;
			}
		}

		protected virtual Vector2 Collide()
		{
			return Collision.noSlopeCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true, true);
		}

	}
}