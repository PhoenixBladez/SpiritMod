using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.SpookySet
{
    public class BatBlaster : ModItem
    {
		private Vector2 newVect;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Blaster");
			Tooltip.SetDefault("Turns bullets into bats");
		}


        public override void SetDefaults()
        {
            item.damage = 55;  
            item.ranged = true;   
            item.width = 65;     
            item.height = 21;    
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
			item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BatBullet"); 
            item.shootSpeed = 6.8f;
            item.useAmmo = AmmoID.Bullet;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			
			Vector2 origVect = new Vector2(speedX, speedY);
			
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(150, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(150, 1800) / 10));
				}
			int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("BatBullet"), damage, knockBack, player.whoAmI);
			
			
			return false;
        }
		  public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1729, 14);
            recipe.AddTile(18);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

    }
}
