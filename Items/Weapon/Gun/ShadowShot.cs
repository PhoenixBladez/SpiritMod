using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class ShadowShot : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadeblaster");
            Tooltip.SetDefault("Converts bullets into vile bullets\nRight-click to shoot out a cursed tracker that sticks to enemies\nVile bullets home onto tracked enemies");
        }


        public override void SetDefaults() {
            item.damage = 22;
            item.ranged = true;
            item.width = 50;
            item.height = 38;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<VileBullet>();
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
            item.crit = 6;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {

                MyPlayer modPlayer = player.GetSpiritPlayer();
                if(modPlayer.shootDelay2 == 0)
                    return true;
                return false;
            } else {
                return true;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY-1)) * 45f;
            if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
                position += muzzleOffset;
            }
            if(player.altFunctionUse == 2) {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 94));
                MyPlayer modPlayer = player.GetSpiritPlayer();
                modPlayer.shootDelay2 = 300;
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ShadowShotTracker>(), item.damage / 3, knockBack, item.owner, 0, 0);
            } else {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 11));
                float spread = 30f * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                {
                    double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                    speedX = baseSpeed * (float)Math.Sin(randomAngle);
                    speedY = baseSpeed * (float)Math.Cos(randomAngle);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<VileBullet>(), item.damage, knockBack, item.owner, 0, 0);
                }
            }
            return false;
        }
        public override void HoldItem(Player player) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(modPlayer.shootDelay2 == 1) {
                Main.PlaySound(25, -1, -1, 1, 1f, 0.0f);
                for(int index1 = 0; index1 < 5; ++index1) {
                    int index2 = Dust.NewDust(player.position, player.width, player.height, 75, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[index2].noLight = false;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.5f;
                }
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
    }
}