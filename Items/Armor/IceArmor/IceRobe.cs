using SpiritMod.Items.Material;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IceArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class IceRobe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blizzard Robes");
            Tooltip.SetDefault("10% increased magic damage\n5% increased magic critical strike chance");
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 18;
            item.value = Item.buyPrice(gold: 8, silver: 60);
            item.rare = 6;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player) {
            player.magicDamage += 0.1f;
            player.magicCrit += 5;

        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<IcyEssence>(), 18);
            recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}