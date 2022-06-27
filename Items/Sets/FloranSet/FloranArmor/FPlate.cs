using SpiritMod.Items.Sets.FloranSet;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet.FloranArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class FPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Plate");
			Tooltip.SetDefault("Increases movement speed by 6%");

		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 18;
			Item.value = Terraria.Item.sellPrice(0, 0, 11, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
			player.maxRunSpeed += .03f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}