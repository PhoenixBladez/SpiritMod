using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class NightSky : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Night Sky");
            Tooltip.SetDefault("Occasionally shoots out powerful stars!");
        }


        int charger;
        public override void SetDefaults() {
            item.damage = 35;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 12.8f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            charger++;
            if(charger >= 4) {
                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromNightSky = true;
                for(int I = 0; I < 4; I++) {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ProjectileID.FallingStar, damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
                Projectile projectile = Main.projectile[p];
                for(int k = 0; k < 30; k++) {
                    Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                    Vector2 offset = mouse - player.position;
                    offset.Normalize();
                    offset *= 20f;
                    int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, 242);

                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = 1.25f;
                    //        Main.dust[dust].scale *= 2f;
                    Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 45f;
                    Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
                }
            } else {
                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromNightSky = true;
            }
            return false;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-6, 0);
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<StarlightBow>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SteamplateBow>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FrostSpine>(), 1);
            recipe.AddIngredient(ItemID.HellwingBow, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}