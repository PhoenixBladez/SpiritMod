using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class TrueHolyBurst : ModItem
    {
        int charger;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Holy Burst");
			Tooltip.SetDefault("Fires three crystal rounds in rapid succession \n Occaisionally fires a Fae Blast that explodes into many wisps of energy");
		}


        public override void SetDefaults()
        {
            item.damage = 33;  
            item.ranged = true;   
            item.width = 50;     
            item.height = 28;    
            item.useTime = 10;
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 1f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
			item.UseSound = SoundID.Item31;
            item.autoReuse = true;
            item.shoot = 89; 
            item.shootSpeed = 9f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 3)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("Fae2"), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 89, damage, knockBack, player.whoAmI);

			return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "HolyBurst", 1);
            recipe.AddIngredient(null, "BrokenParts", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
