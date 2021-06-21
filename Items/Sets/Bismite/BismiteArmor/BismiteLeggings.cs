using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Bismite.BismiteArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BismiteLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Leggings");
			Tooltip.SetDefault("6% increased movement speed");

		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.buyPrice(silver: 6);
			item.rare = ItemRarityID.Blue;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
