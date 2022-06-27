
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops.InfernonArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class InfernalGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Monger's Greaves");
			Tooltip.SetDefault("Increases magic critical chance by 7% and reduces mana consumption by 10%");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.rare = ItemRarityID.Pink;
			Item.value = 42000;

			Item.defense = 9;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 7;
			player.manaCost -= 0.10f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<InfernalAppendage>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

	}
}
