using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ChitinLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Leggings");
			Tooltip.SetDefault("Increases movement speed by 7%");

		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(silver: 10);
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.07f;
			player.maxRunSpeed += 0.07f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
