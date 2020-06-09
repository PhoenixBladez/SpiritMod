using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class TrueDarkStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("True Horizon's Edge");
            Tooltip.SetDefault("Shoots out a splitting volley of homing pestilence and cursed fire.");
        }


        public override void SetDefaults() {
            item.damage = 62;
            item.magic = true;
            item.mana = 15;
            item.width = 66;
            item.height = 68;
            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<CursedFire>();
            item.shootSpeed = 16f;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<BrokenStaff>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<NightStaff>(), 1);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}