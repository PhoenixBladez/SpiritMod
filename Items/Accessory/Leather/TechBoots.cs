
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shoes)]
    public class TechBoots : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Coiled Cleats");
            Tooltip.SetDefault("Increases movement speed and acceleration slightly\nIncreases movement speed and acceleration further when below half health");
        }
        public override void SetDefaults() {
            item.width = 28;
            item.height = 20;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.rare = 1;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.moveSpeed += 0.09f;
            player.runAcceleration += .05f;
            if(player.statLife <= player.statLifeMax2 / 2) {
                player.moveSpeed += 0.09f;
                player.runAcceleration += .05f;
                if(player.velocity.X != 0f) {
                    int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 226);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LeatherBoots>(), 1);
            recipe.AddIngredient(null, "TechDrive", 5);
            recipe.AddRecipeGroup("EvilMaterial1", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
