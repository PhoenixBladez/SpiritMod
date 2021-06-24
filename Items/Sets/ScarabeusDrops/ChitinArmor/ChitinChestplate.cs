using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
namespace SpiritMod.Items.Sets.ScarabeusDrops.ChitinArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class ChitinChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Chestplate");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(silver: 12);
			item.rare = ItemRarityID.Blue;
			item.defense = 4;
		}

		public override bool DrawBody() => false;
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
