using System;
using Terraria;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class PhantomArc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Arc");
			Tooltip.SetDefault("Summons an infinitely piercing laser of lost souls");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.value = Item.buyPrice(0, 6, 0, 0);
			item.rare = 5;
			item.damage = 34;
            item.mana = 9;
            item.useStyle = 5;
			item.useTime = 10;
			item.useAnimation = 7;
			//item.scale = 0.9f;
			item.reuseDelay = 5;
			item.magic = true;
			item.channel = true;
			item.noMelee = true;
			//item.noUseGraphic = true;
			item.shoot = mod.ProjectileType("PhantomArcHandle");
			item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "SpiritBar", 8);
            modRecipe.AddIngredient(null, "SoulShred", 6);
            modRecipe.AddIngredient(531, 1);
			modRecipe.AddTile(101);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
