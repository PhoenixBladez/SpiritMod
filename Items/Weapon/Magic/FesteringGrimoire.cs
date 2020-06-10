using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class FesteringGrimoire : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Festering Grimoire");
            Tooltip.SetDefault("Shoots out a rapidly accelerating cursed sickle!");
        }


        public override void SetDefaults() {
            item.damage = 39;
            item.magic = true;
            item.mana = 13;
            item.width = 40;
            item.height = 40;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<GrimoireScythe>();
            item.shootSpeed = 2f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<PutridPiece>(), 8);
            recipe.AddIngredient(531, 1);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}