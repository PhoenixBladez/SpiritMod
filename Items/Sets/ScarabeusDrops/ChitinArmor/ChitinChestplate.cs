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
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 12);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
