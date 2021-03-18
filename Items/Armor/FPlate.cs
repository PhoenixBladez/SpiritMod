
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class FPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Plate");
			Tooltip.SetDefault("Increases movement speed by 6%");

		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 18;
			item.value = Terraria.Item.sellPrice(0, 0, 11, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
			player.maxRunSpeed += .03f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}