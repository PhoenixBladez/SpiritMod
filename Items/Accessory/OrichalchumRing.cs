using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class OrichalchumRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Ring");
			Tooltip.SetDefault("Attacking foes can send a fast petal across the screen");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 4;

			item.accessory = true;

			item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MyPlayer>(mod).OriRing = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
