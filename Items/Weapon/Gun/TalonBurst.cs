using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Gun
{
    public class TalonBurst : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Burst");
			Tooltip.SetDefault("Shoots out two bullets in quick succession");
		}


        int charger;
        private int memes;
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;   
            item.width = 65;     
            item.height = 21;    
            item.useTime = 12;
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item36;
            item.autoReuse = false;
            item.shoot = 10; 
			item.crit = 8;
            item.shootSpeed = 5f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 5)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("GiantFeather"), 14, knockBack, player.whoAmI, 0f, 0f);
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
            recipe.AddIngredient(null, "Talon", 14);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
