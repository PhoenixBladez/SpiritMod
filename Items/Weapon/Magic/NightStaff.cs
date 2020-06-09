using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class NightStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Horizon's Edge");
            Tooltip.SetDefault("Summons a portal at the cursor position that shoots homing energy at enemies\nRight click to cause all portals to explode into a shower of stars\nUp to 2 portals can exist at once");
        }


        public override void SetDefaults() {
            item.damage = 35;
            item.magic = true;
            item.width = 44;
            item.height = 48;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 4;
            item.UseSound = SoundID.Item20;
            item.mana = 14;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<CorruptPortal>();
            item.shootSpeed = 13f;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<CorruptStaff>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<JungleStaff>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<DungeonStaff>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<HellStaff>(), 1);
            modRecipe.AddTile(26);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {
                {
                    for(int projFinder = 0; projFinder < 300; ++projFinder) {
                        if(Main.projectile[projFinder].type == type) {
                            Main.projectile[projFinder].aiStyle = -3;
                            Main.projectile[projFinder].Kill();
                        }

                    }
                }
                return false;
            } else {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}