using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
    public class Twilight1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Twilight Talisman");
            Tooltip.SetDefault("Increases melee speed by 5%\nIncreases damage reduction and damage dealt by 5%\nIncreases critical strike chance by 4%\nIncreases movement speed by 10%\nAttacks have a small chance of inflicting Shadowflame");
        }


        public override void SetDefaults() {
            base.item.width = 14;
            base.item.height = 24;
            base.item.rare = 3;
            base.item.UseSound = SoundID.Item11;
            base.item.accessory = true;
            base.item.value = Item.buyPrice(0, 2, 30, 0);
            base.item.value = Item.sellPrice(0, 1, 6, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.meleeDamage += 0.05f;
            player.magicDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.minionDamage += 0.05f;
            player.thrownDamage += 0.05f;
            player.meleeCrit += 4;
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.endurance += 0.05f;
            player.meleeSpeed += 0.05f;
            player.moveSpeed += .1f;
            player.GetSpiritPlayer().twilightTalisman = true;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "YoyoCharm2", 1);
            modRecipe.AddIngredient(null, "MCharm", 1);
            modRecipe.AddIngredient(null, "DCharm", 1);
            modRecipe.AddIngredient(null, "HCharm", 1);
            modRecipe.AddTile(TileID.DemonAltar);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
