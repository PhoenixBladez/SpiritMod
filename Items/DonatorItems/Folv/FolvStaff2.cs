
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	[AutoloadEquip(EquipType.Balloon)]
	public class FolvStaff2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Worn Staff of Defense");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = ItemRarityID.LightRed;
			item.value = 5000;
            item.vanity = true;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddIngredient(ModContent.ItemType<FolvStaff1>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
