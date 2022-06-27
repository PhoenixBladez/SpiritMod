
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class GeodeHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Helmet");
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 30;
			Item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.LightRed;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 2);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
