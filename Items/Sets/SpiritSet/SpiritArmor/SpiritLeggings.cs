using SpiritMod.Items.Sets.SpiritSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet.SpiritArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SpiritLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Leggings");
			Tooltip.SetDefault("10% increased melee speed");

		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 34;
			Item.value = 30000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += 0.10f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}