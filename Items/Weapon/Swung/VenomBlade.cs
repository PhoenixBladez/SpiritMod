using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class VenomBlade : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Venom Blade");
        }


        public override void SetDefaults() {
            item.damage = 47;
            item.useTime = 26;
            item.useAnimation = 26;
            item.melee = true;
            item.width = 60;
            item.height = 64;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
            item.shootSpeed = 10;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = 355;
        }

        public override bool OnlyShootOnSwing => true;

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpiderFang, 12);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}