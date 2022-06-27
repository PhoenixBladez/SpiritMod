using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet.BismiteArmor
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
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(silver: 6);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
