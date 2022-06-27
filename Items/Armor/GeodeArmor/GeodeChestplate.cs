using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class GeodeChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Chestplate");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.LightRed;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 35);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
