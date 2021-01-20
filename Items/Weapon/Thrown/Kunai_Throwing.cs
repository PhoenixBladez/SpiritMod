using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Kunai_Throwing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kunai");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 9;
			item.height = 15;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.consumable = true;
			item.maxStack = 999;
			item.shoot = mod.ProjectileType("Kunai_Throwing");
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 7.5f;
			item.damage = 12;
			item.knockBack = 1.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 1, 0);
			item.crit = 8;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 1);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 1);
			recipe.AddIngredient(ItemID.LeadBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 direction = new Vector2(speedX,speedY);

			Terraria.Projectile.NewProjectile(position, direction.RotatedBy(-0.2f), mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position, direction, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position, direction.RotatedBy(0.2f), mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
