using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MarbleArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class MarbleChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Robe");
			Tooltip.SetDefault("");

		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 12100;
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
