
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class AdamantiteRing : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Adamantite Band");
            Tooltip.SetDefault("-8 defense\n10% increased damage and critical strike chance when under half health");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = 4;
            item.defense = -8;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(player.statLife < player.statLifeMax2 / 2) {
                player.magicCrit += 10;
                player.meleeCrit += 10;
                player.rangedCrit += 10;
                player.magicDamage *= 1.1f;
                player.meleeDamage *= 1.1f;
                player.rangedDamage *= 1.1f;
                player.minionDamage *= 1.1f;
            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
