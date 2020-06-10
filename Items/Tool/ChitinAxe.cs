using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class ChitinAxe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Chitin Axe");
        }


        public override void SetDefaults() {
            item.width = 46;
            item.height = 46;
            item.value = 10000;
            item.rare = 1;
            item.axe = 9;
            item.damage = 12;
            item.knockBack = 6;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 22;
            item.useAnimation = 22;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}