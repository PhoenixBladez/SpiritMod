using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DunePlate : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dune Plate");
        }


        int timer = 0;
        public override void SetDefaults() {
            item.width = 20;
            item.height = 18;
            item.value = 10000;
            item.rare = 6;
            item.vanity = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence", 3);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}