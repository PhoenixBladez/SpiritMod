using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class DodgeBall1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Sluggy Throw");
            Tooltip.SetDefault("Throw a dodgeball at snail speed");
        }


        public override void SetDefaults() {
            item.damage = 13;
            item.melee = true;
            item.width = 30;
            item.height = 30;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 9;
            item.value = 4000;
            item.rare = 2;
            item.shootSpeed = 6f;
            item.shoot = mod.ProjectileType("Dodgeball1");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 11);
            recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "DodgeBall", 1);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}