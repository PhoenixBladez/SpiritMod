using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class StellarPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Plate");
			Tooltip.SetDefault("Increases minion damage by 10%\nIncreases your max number of minions");

		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
			recipe.AddIngredient(ItemID.TitaniumBar, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
			recipe1.AddIngredient(ItemID.AdamantiteBar, 20);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.Register();
		}
	}
}
