using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MageTree
{
    [AutoloadEquip(EquipType.Neck)]
    public class ArcaneNecklace : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Necklace");
			Tooltip.SetDefault("Increases maximum mana by 20\nEnemies have 20% chance to drop an extra Mana Star");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 42;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual) => player.statManaMax2 += 20;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 3);
			recipe.AddIngredient(ItemID.Chain, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
