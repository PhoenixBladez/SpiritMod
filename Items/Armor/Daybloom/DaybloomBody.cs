
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Daybloom
{
    [AutoloadEquip(EquipType.Body)]
    public class DaybloomBody : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Daybloom Garb");
            Tooltip.SetDefault("Increases magic damage by 1");
        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.rare = 0;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player) {
            player.GetSpiritPlayer().daybloomGarb = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Daybloom, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
