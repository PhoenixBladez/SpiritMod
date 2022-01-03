using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WayfarerSet
{
    [AutoloadEquip(EquipType.Legs)]
    public class WayfarerLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wayfarer's Pants");
            Tooltip.SetDefault("7% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.07f;
            player.runAcceleration += .015f;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 1);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
