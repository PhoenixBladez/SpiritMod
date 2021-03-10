using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Prim;

namespace SpiritMod.Items.GamblerChestLoot.Champagne
{
	public class Champagne : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Champagne");
			Tooltip.SetDefault("Hold down and release to pop the cork");
		}
		public override void SetDefaults()
		{
			item.useStyle = 100;
			item.width = 40;
			item.height = 32;
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.useAnimation = 320;
			item.useTime = 320;
			item.shootSpeed = 8f;
			item.value = Item.sellPrice(0, 0, 0, 1);
			item.rare = ItemRarityID.Blue;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = ModContent.ProjectileType<ChampagneProj>();
			item.maxStack = 999;
		}
	}
	public class ChampagneProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Champagne");
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
			projectile.melee = true;
			projectile.ownerHitCheck = true;
		}
		bool released = false;
		bool stopped = false;
		Vector2 direction;
		private float charge
		{
			get{return projectile.ai[0]; }
			set{projectile.ai[0] = value; }
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player == Main.player[Main.myPlayer])
			{
			if (Main.MouseWorld.X > player.Center.X)
				player.direction = 1;
			else
				player.direction = -1;
			}
			projectile.Center = player.Center;
			if (!stopped)
			{
				direction = Main.MouseWorld - player.Center;
				direction.Normalize();
				player.itemAnimation -= (int)((charge + 50) / 6);
				while (player.itemAnimation < 3)
				{
					Main.PlaySound(SoundID.Item1, projectile.Center);
					if (released && charge < 100)
					{
						projectile.active = false;
						player.itemAnimation = 2;
						player.itemTime = 2;
						break;
					}
					else
						player.itemAnimation += 320;
				}
				if (charge < 100)
					charge++;
				if (charge == 99)
					Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
				if (!player.channel || released)
					released = true;
				if (released)
				{
					int degrees = (int)(((player.itemAnimation) * -0.7) + 55) * player.direction * (int)player.gravDir;
					if (player.direction == 1)
					{
						degrees += 180;
					}
					else
					{
						degrees += 90;
					}
					double radians = degrees * (Math.PI / 180);
					double throwingAngle = direction.ToRotation() + 3.9;
					if (Math.Abs(radians - throwingAngle) < 0.15f && charge >= 100)
					{
						stopped = true;
						projectile.timeLeft = 60;
						Projectile.NewProjectile(player.Center, direction * (charge / 6f), ModContent.ProjectileType<ChampagneCork>(), 1, projectile.knockBack, player.whoAmI);
						//for (int i = 0; i < 15; i++)
							Projectile.NewProjectile(player.Center + direction * 20, direction * (float)Math.Sqrt(projectile.timeLeft), ModContent.ProjectileType<ChampagneLiquid>(), projectile.damage, projectile.knockBack, player.whoAmI);
					}
				}
			}
			else
			{
				if (projectile.timeLeft % 2 == 0)
					Projectile.NewProjectile(player.Center + direction * 20, direction * (float)Math.Sqrt(projectile.timeLeft), ModContent.ProjectileType<ChampagneLiquid>(), projectile.damage, projectile.knockBack, player.whoAmI);
				player.itemAnimation++;
				if (projectile.timeLeft == 2)
				{
					player.HeldItem.stack -= 1;
					player.itemAnimation = 2;
					projectile.active = false;
				}
			}
			player.itemTime = player.itemAnimation;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
	}
	public class ChampagneCork : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Champagne");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults() {
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
			projectile.ranged = true;
			projectile.aiStyle = 1;
			projectile.penetrate = 1;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);
			for(int i = 0; i < 8; i++)
			{
				int dusttype = (Main.rand.Next(3) == 0) ? 1 : 7;
				Vector2 velocity = projectile.velocity.RotatedByRandom(Math.PI / 16) * Main.rand.NextFloat(0.3f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, dusttype, velocity.X, velocity.Y);
			}
		}
	}
	public class ChampagneLiquid : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Champagne");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults() {
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
			projectile.ranged = true;
			projectile.aiStyle = 1;
			projectile.penetrate = 1;
			projectile.alpha = 255;
		}
		bool primsCreated = false;
		public override void AI()
		{
			if (!primsCreated)
			{
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new ChampagnePrimTrail(projectile, new Color(250, 214, 165), (int)(2.5f * projectile.velocity.Length()), Main.rand.Next(15,25)));
			}
		}
	}
}