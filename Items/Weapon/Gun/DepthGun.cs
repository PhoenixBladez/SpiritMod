using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class DepthGun : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Depth Pistol");
            Tooltip.SetDefault("Converts bullets into Depth Rounds\nDepth bullets occasionally explode into seawater, damaging nearby enemies");

        }


        public override void SetDefaults() {
            item.damage = 40;
            item.ranged = true;
            item.width = 65;
            item.useAmmo = AmmoID.Bullet;
            item.height = 21;
            item.useTime = 21;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 70, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<DepthBullet>();
            item.shootSpeed = 15f;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DepthBullet>(), damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}