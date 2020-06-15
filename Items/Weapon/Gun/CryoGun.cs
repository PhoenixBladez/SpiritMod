using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
    public class CryoGun : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Cryoblaster");
            Tooltip.SetDefault("Hold and release to launch a barrage of Cyrolite bullets, inflicting 'Cryo Crush'\n'Cryo Crush' deals more damage the less life your enemies have left\nThis effect does not work on bosses, instead dealing a flat raate of damage");
        }
        public override void SetDefaults() {
            item.damage = 125;
            item.noMelee = true;
            item.channel = true; //Channel so that you can held the weapon [Important]
            item.rare = 5;
            item.width = 18;
            item.height = 18;
            item.useTime = 20;
            item.knockBack = 2.5f;
            item.UseSound = SoundID.Item13;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<StarMapProj>();
            item.shootSpeed = 0f;
            item.noUseGraphic = true;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
         public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
                type = ModContent.ProjectileType<CryoBlasterProj>();
                return true;
            }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}