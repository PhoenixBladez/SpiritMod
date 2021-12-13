using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Magic.CreepingVine
{

	//TODO: Make it so you cant throw one if theres one active
	//TODO: Make it so right click destroys the currently alive one
	public class CreepingVine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeping Vine");
			Tooltip.SetDefault("Throw at a surface to grow damaging vines");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
			item.noMelee = true;
			item.rare = ItemRarityID.Green;
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
			item.shoot = ModContent.ProjectileType<CreepingVineProj>();
			item.shootSpeed = 20f;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.mana = 20;
		}
	}
	public class CreepingVineProj : ModProjectile
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
			projectile.magic = true;
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
				Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, ModContent.ProjectileType<CreepingVineProjTwo>(), projectile.damage, projectile.knockBack, projectile.owner);
				if (proj.modProjectile is CreepingVineProjTwo modproj)
				{
					velocity = velocity.RotatedBy(-1.57f / 2);
					velocity.Normalize();
					modproj.moveDirection = velocity;
				}
				proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, ModContent.ProjectileType<CreepingVineProjTwo>(), projectile.damage, projectile.knockBack, projectile.owner);
				if (proj.modProjectile is CreepingVineProjTwo modproj2)
				{
					velocity = velocity.RotatedBy(1.57f);
					velocity.Normalize();
					modproj2.moveDirection = velocity;
				}
				projectile.Center += projectile.velocity;
				projectile.velocity = Vector2.Zero;
			}
			return false;
		}
	}
	public class CreepingVineProjTwo : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vine");


		public CreepingVinePrimTrail trail;
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
			projectile.magic = true;
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
				trail = new CreepingVinePrimTrail(projectile);

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
				points.Add(projectile.Center);
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

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			foreach (Vector2 point in points)
			{
				if (point.X > targetHitbox.Left && point.X < targetHitbox.Right && point.Y > targetHitbox.Top && point.Y < targetHitbox.Bottom)
					return true;
			}
			return false;
		}

	}
}