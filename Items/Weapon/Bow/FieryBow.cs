using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class FieryBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Bow");
			Tooltip.SetDefault("Shoots out two powerful flaming arrows");
		}



        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 40;
            item.useTime = 22;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.shoot = 9;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 25f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 2, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}