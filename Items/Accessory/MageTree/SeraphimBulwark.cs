using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MageTree
{
    [AutoloadEquip(EquipType.Shield)]
    public class SeraphimBulwark : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Bulwark");
			Tooltip.SetDefault("Increases maximum mana by 40\nAbsorbs 10% of the damage dealt by enemies\nThis damage is converted into a loss of mana instead");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Green;
            Item.defense = 4;
			Item.accessory = true;
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual) => player.statManaMax2 += 40;

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ArcaneNecklace>());
            recipe.AddIngredient(ModContent.ItemType<ManaShield>());
            recipe.AddIngredient(ModContent.ItemType<Sets.SpiritSet.SoulShred>(), 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
