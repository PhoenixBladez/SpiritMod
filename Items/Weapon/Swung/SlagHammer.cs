using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class SlagHammer : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Slag Breaker");
            Tooltip.SetDefault("Hold 'down' to keep swinging\nThis hammer explodes after hitting 4 targets\nHitting enemies releases damaging sparks\nRight click to throw the Hammer like a boomerang");
        }


        private Vector2 newVect;
        public override void SetDefaults() {
            item.useStyle = 100;
            item.width = 40;
            item.height = 32;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.useAnimation = 44;
            item.useTime = 44;
            item.shootSpeed = 8f;
            item.knockBack = 5f;
            item.damage = 29;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = 3;
            item.shoot = ModContent.ProjectileType<SlagHammerProj>();
        }
        public override bool UseItemFrame(Player player) {
            if(player.altFunctionUse != 2) {
                player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            }
            return true;

        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }


        public override bool CanUseItem(Player player) {
            for(int i = 0; i < 1000; ++i) {
                if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
                    return false;
                }
            }
            if(player.altFunctionUse == 2) {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.shoot = ModContent.ProjectileType<SlagHammerProjReturning>();
            } else {
                item.useTime = 46;
                item.useAnimation = 46;
                item.shoot = ModContent.ProjectileType<SlagHammerProj>();
            }
            return base.CanUseItem(player);
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 16);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}