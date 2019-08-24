using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class FrostStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icy Gale");
			Tooltip.SetDefault("Shoots two homing icicles.");
		}


		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.mana = 15;
			item.width = 54;
			item.height = 54;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = 80000;
			item.rare = 6;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("IceSpike");
			item.shootSpeed = 34f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 10);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float spread = 60 * 0.0174f; //change 60 to degrees you want
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
			double deltaAngle = spread / 3; //change 5 to what you wan the number to be
			for (int i = 0; i < 3; i++)//change 5 to what you wan the number to be
			{
				double offsetAngle = startAngle + deltaAngle * i;
				Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), item.shoot, damage, knockBack, player.whoAmI);
			}
			return false;
		}

	}
}
