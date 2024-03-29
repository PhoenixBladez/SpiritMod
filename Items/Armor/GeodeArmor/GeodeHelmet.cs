
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
			item.width = 40;
			item.height = 30;
			item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.LightRed;

			item.vanity = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalShard, 2);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
