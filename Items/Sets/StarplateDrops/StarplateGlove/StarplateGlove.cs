using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplateGlove
{
	public class StarplateGlove : ModItem
	{
		float charge = 1.33f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hundred-Crack Fist");
			Tooltip.SetDefault("Right click launch multiple punches outward");
		}

		public override void SetDefaults()
		{
			Item.shootSpeed = 10f;
			Item.damage = 31;
			Item.knockBack = 3.3f;
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 7;
			Item.useTime = 7;
			Item.channel = true;
			Item.width = 26;
			Item.height = 26;
			Item.mana = 6;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(silver: 55);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<StargloveChargeOrange>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 17);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse != 2)
			{
				for (int i = 0; i < 1000; ++i)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<StarplateGloveProj>())
						return false;
				}
				Item.useTime = 7;
				Item.useAnimation = 7;
			}
			else
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
			}

			return true;
		}
		public override void HoldItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<StarplateGloveProj>())
					return;

			if (charge > 0)
			{
				int chosenDust = Main.rand.Next(2) == 0 ? 6 : 62;
				Vector2 vector2_1 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
				if (player.direction != 1)
					vector2_1.X = (float)player.bodyFrame.Width - vector2_1.X;
				if ((double)player.gravDir != 1.0)
					vector2_1.Y = (float)player.bodyFrame.Height - vector2_1.Y;
				Vector2 vector2_2 = player.RotatedRelativePoint(player.position + vector2_1 - new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f, true) - player.velocity;
				for (int index = 0; index < 4; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, chosenDust, (float)(player.direction * 2), 0.0f, 150, new Color(), 1.3f)];
					dust.position = vector2_2;
					dust.velocity *= 0.0f;
					dust.noGravity = true;
					dust.fadeIn = 1f;
					dust.velocity += player.velocity;
					dust.scale *= charge;
					if (Main.rand.Next(2) == 0)
					{
						dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
						dust.scale += Main.rand.NextFloat();
						if (Main.rand.Next(2) == 0)
							dust.customData = (object)player;
					}
				}
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<StarplateGloveProj>()] != 0)
				return false;

			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<StarplateGloveProj>(), damage, knockback, player.whoAmI);
			}
			else
			{
				float stray = Main.rand.NextFloat(-0.5f, 0.5f);
				Vector2 speed = new Vector2(speedX, speedY).RotatedBy(stray);
				//speed *= Main.rand.NextFloat(0.9f, 1.1f);
				position += speed * 8;
				type = Main.rand.Next(2) == 0 ? ModContent.ProjectileType<StargloveChargeOrange>() : ModContent.ProjectileType<StargloveChargePurple>();
				int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);

				if (type == ModContent.ProjectileType<StargloveChargePurple>())
				{
					for (float num2 = 0.0f; (double)num2 < 10; ++num2)
					{
						int dustIndex = Dust.NewDust(position - speed * 3, 2, 2, DustID.Clentaminator_Cyan, 0f, 0f, 0, default, 1.5f);
						Main.dust[dustIndex].noGravity = true;
						Main.dust[dustIndex].velocity = Vector2.Normalize((speed * 5).RotatedBy(Main.rand.NextFloat(6.28f))) * 2.5f;
					}
					for (int j = 0; j < 5; j++)
						Projectile.NewProjectile(position, speed, type, 0, 0, player.whoAmI, proj);
				}
				else
				{
					for (float num2 = 0.0f; (double)num2 < 10; ++num2)
					{
						int dustIndex = Dust.NewDust(position - speed * 3, 2, 2, DustID.Torch, 0f, 0f, 0, default, 2f);
						Main.dust[dustIndex].noGravity = true;
						Main.dust[dustIndex].velocity = Vector2.Normalize((speed * 8).RotatedBy(Main.rand.NextFloat(6.28f))) * 2.5f;
					}
					for (int j = 0; j < 5; j++)
						Projectile.NewProjectile(position, speed, type, 0, 0, player.whoAmI, proj);
				}
			}
			return false;
		}
	}
}