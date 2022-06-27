using SpiritMod.Items.Sets.RunicSet;
using SpiritMod.Items.Sets.SpiritSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RunicSet.RunicArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class RunicPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Plate");
			Tooltip.SetDefault("Increases magic critical strike chance by 8%");

		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Magic) += 8;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 16);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}