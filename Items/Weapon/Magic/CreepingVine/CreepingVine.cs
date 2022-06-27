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
			Item.damage = 17;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Green;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 8;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<CreepingVineProj>();
			Item.shootSpeed = 20f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.mana = 20;
		}
	}
	public class CreepingVineProj : ModProjectile
	{
		bool stuck = false;
		float shrinkCounter = 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			AIType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (stuck)
			{
				Projectile.velocity = Vector2.Zero;
				if (Projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					Projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (Projectile.scale < 0.3f)
					{
						Projectile.active = false;
					}
					if (Projectile.scale > 1)
						Projectile.scale = ((Projectile.scale - 1) / 2f) + 1;
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
				Projectile.tileCollide = false;
				Projectile.timeLeft = 375;
				Vector2 velocity = new Vector2(oldVelocity.X == Projectile.velocity.X ? 0 : Math.Sign(oldVelocity.X), oldVelocity.Y == Projectile.velocity.Y ? 0 : Math.Sign(oldVelocity.Y));
				//Main.NewText("X: " + velocity.X.ToString());
				//Main.NewText("Y: " + velocity.Y.ToString());
				Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<CreepingVineProjTwo>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				if (proj.ModProjectile is CreepingVineProjTwo modproj)
				{
					velocity = velocity.RotatedBy(-1.57f / 2);
					velocity.Normalize();
					modproj.moveDirection = velocity;
				}
				proj = Projectile.NewProjectileDirect(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<CreepingVineProjTwo>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				if (proj.ModProjectile is CreepingVineProjTwo modproj2)
				{
					velocity = velocity.RotatedBy(1.57f);
					velocity.Normalize();
					modproj2.moveDirection = velocity;
				}
				Projectile.Center += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
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
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = Projectile.height = 8;
			Projectile.timeLeft = 750;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			if (!primsCreated)
			{
				trail = new CreepingVinePrimTrail(Projectile);

				SpiritMod.primitives.CreateTrail(trail);
				primsCreated = true;
			}

			if (growCounter < 1)
				Projectile.scale = growCounter += 0.1f;

			newVelocity = Collide();
			if (Math.Abs(newVelocity.X) < 0.5f)
				collideX = true;
			else
				collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				collideY = true;
			else
				collideY = false;

			if (Projectile.ai[1] == 0f)
			{
				Projectile.rotation += (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideY)
				{
					Projectile.ai[0] = 2f;
				}
				if (!collideY && Projectile.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 1f;
					Projectile.ai[0] = 1f;
				}
				if (collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 1f;
				}
			}
			else
			{
				Projectile.rotation -= (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideX)
				{
					Projectile.ai[0] = 2f;
				}
				if (!collideX && Projectile.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 0f;
					Projectile.ai[0] = 1f;
				}
				if (collideY)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 0f;
				}
			}
			if (Projectile.timeLeft > 600)
			{
				Projectile.velocity = speed * moveDirection;
				Projectile.velocity = Collide();
				points.Add(Projectile.Center);
				trail._addPoints = true;
			}
			else
			{
				Projectile.velocity = Vector2.Zero;
				trail._addPoints = false;
			}
		}

		protected virtual Vector2 Collide()
		{
			return Collision.noSlopeCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, true, true);
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