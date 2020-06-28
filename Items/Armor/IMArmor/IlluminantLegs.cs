using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class IlluminantLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Boots");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 16;
			item.value = 90000;
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.SoulofLight, 4);
			recipe.AddIngredient(ModContent.ItemType<Geode>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
