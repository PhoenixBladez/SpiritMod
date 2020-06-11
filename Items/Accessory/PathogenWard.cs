
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class PathogenWard : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Pathogen Ward");
            Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nKilling enemies within the aura restores some life\nHearts are more likely to drop from enemies\nProvides immunity to the 'Poisoned' buff");

        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.rare = 4;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().Ward = true;
            player.GetSpiritPlayer().vitaStone = true;
            player.buffImmune[BuffID.Poisoned] = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Bloodstone>(), 1);
            recipe.AddIngredient(ItemID.Bezoar, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
