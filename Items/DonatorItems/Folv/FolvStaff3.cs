using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	[AutoloadEquip(EquipType.Balloon)]
	public class FolvStaff3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Worn Staff of Protection");
		}

		public override void SetDefaults()
		{
			item.width = 60;
			item.height = 60;
			item.rare = ItemRarityID.Lime;
			item.value = 95000;
			item.accessory = true;
            item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddIngredient(ModContent.ItemType<FolvStaff2>(), 1);
			recipe.AddIngredient(ModContent.ItemType<IcyEssence>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
