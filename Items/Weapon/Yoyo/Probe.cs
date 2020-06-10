using SpiritMod.Items.Placeable.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
    public class Probe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Probe");
            Tooltip.SetDefault("Shoots out lasers in random arcs");
        }


        public override void SetDefaults() {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 52;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            base.item.rare = 6;
            base.item.knockBack = 3f;
            base.item.channel = true;
            base.item.useStyle = ItemUseStyleID.HoldingOut;
            base.item.useAnimation = 25;
            base.item.useTime = 24;
            base.item.shoot = ModContent.ProjectileType<ProbeP>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 11);
            recipe.AddIngredient(ItemID.SoulofMight, 13);
            recipe.AddIngredient(ModContent.ItemType<PrintProbe>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
