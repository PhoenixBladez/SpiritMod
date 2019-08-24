using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class CryoGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Cryoblaster");
			Tooltip.SetDefault("Every sixth bullet shot out is a Cryolite Bullet that explodes upon hitting foes, inflicting 'Cryo Crush'\n'Cryo Crush' deals more damage the less life your enemies have left\nThis effect does not work on bosses, instead dealing a flat raate of damage");
		}
        int charger;

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 28;
            item.height = 14;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 32, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 6)
            {
                {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("CryoliteBullet"), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CryoliteBar", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}