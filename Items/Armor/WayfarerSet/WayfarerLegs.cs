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
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.07f;
            player.runAcceleration += .015f;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 1);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
