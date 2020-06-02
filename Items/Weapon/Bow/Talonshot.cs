using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Talonshot : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talonshot");
			Tooltip.SetDefault("Shoots a burst feather");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 28;
            item.useTime = 29;
            item.useAnimation = 29;
            item.useStyle = 5;
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
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("FeatherArrow"), damage, knockBack, player.whoAmI);
			return false;
		}
		
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 14);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}