using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DepthGreaves : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Depth Walker's Greaves");
            Tooltip.SetDefault("10% increased melee speed\nIncreases your max number of minions\nSlightly increases the knockback of your minions");

        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.value = Item.buyPrice(silver: 60);
            item.rare = 5;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player) {
            player.minionKB += 0.5f;
            player.meleeSpeed += 0.10f;
            player.maxMinions += 1;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DepthShard>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
