using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{

	[AutoloadEquip(EquipType.Legs)]
	public class StellarLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Leggings");
			Tooltip.SetDefault("Increases minion knocback by 20%\nIncreases flight time by 15%");

		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionKB += 0.15f;
			player.wingTimeMax += player.wingTimeMax + (int)(player.wingTimeMax * .15f);
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
            recipe.AddIngredient(ItemID.TitaniumBar, 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
            recipe1.AddIngredient(ItemID.AdamantiteBar, 16);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();
        }
    }
}
