using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class HolyBurst : ModItem

    {
        private Vector2 newVect;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Burst");
			Tooltip.SetDefault("Fires three crystal rounds in rapid succession");
		}

        public override void SetDefaults()
        {
            item.damage = 26;  
            item.ranged = true;   
            item.width = 50;     
            item.height = 28;    
            item.useTime = 10;
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 0.4f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item31;
            item.autoReuse = true;
            item.shoot = 89; 
            item.shootSpeed = 11f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        { 
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 89, damage, knockBack, player.whoAmI);
        }
			return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
