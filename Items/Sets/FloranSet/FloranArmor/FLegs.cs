using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Items.Sets.FloranSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet.FloranArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class FLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Leggings");
			Tooltip.SetDefault("Increases movement speed by 4%");

		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 18;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .04f;
		}

		public override void AddRecipes()  //How to craft this item
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 10);   //you need 10 Wood
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 7);
			recipe.AddTile(TileID.Anvils);   //at work bench
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}