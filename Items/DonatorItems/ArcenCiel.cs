using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Items.DonatorItems
{
	public class ArcenCiel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc-en-Ciel");
			Tooltip.SetDefault("Summons a RAINBOW of lost souls\nLasts for five seconds before cancelling out\nInflicts 'Electrified' and 'Holy Light'\n~Donator Item~");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.value = Item.buyPrice(0, 6, 0, 0);
			item.rare = 7;
			item.damage = 38;
            item.mana = 12;
            item.useStyle = 5;
			item.useTime = 10;
			item.useAnimation = 7;
            item.reuseDelay = 5;
			item.magic = true;
			item.channel = true;
			item.noMelee = true;
			//item.noUseGraphic = true;
			item.shoot = mod.ProjectileType("ArcEnCielHandle");
			item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "PhantomArc", 1);
            modRecipe.AddIngredient(ItemID.ChlorophyteBar, 3);
			modRecipe.AddIngredient(ItemID.SoulofLight, 5);
			modRecipe.AddIngredient(ItemID.SoulofSight, 5);
            modRecipe.AddIngredient(ItemID.PixieDust, 8);
            modRecipe.AddTile(101);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
