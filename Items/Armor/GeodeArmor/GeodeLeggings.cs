using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class GeodeLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Leggings");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.LightRed;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 3);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
