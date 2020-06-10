using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Talonshot : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Talonshot");
            Tooltip.SetDefault("Shoots a burst feather");
        }


        int charger;
        public override void SetDefaults() {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 28;
            item.useTime = 29;
            item.useAnimation = 29;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = 1000;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 8f;

        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-6, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<FeatherArrow>(), damage, knockBack, player.whoAmI);
            return false;
            Projectile projectile = Main.projectile[proj];
            for(int k = 0; k < 15; k++) {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Vector2 offset = mouse - player.position;
                offset.Normalize();
                offset *= 23f;
                int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, 42);

                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                //        Main.dust[dust].scale *= 2f;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 34f;
                Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Talon>(), 14);
            recipe.AddIngredient(ModContent.ItemType<FossilFeather>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}