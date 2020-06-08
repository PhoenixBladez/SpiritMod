using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.QuicksilverArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class QuicksilverLegs : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Quicksilver Greaves");
            Tooltip.SetDefault("Increases movement speed by 25%\nAllows detection of ores\nReduces mana cost by 5%");

        }

        public override void SetDefaults() {
            item.width = 26;
            item.height = 12;
            item.value = 10000;
            item.rare = 8;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.25f;
            player.maxRunSpeed += 2;
            player.AddBuff(BuffID.Spelunker, 1);
            player.manaCost -= .05f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}