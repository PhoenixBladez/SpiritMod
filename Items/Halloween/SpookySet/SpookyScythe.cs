using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.SpookySet
{
    public class SpookyScythe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Scythe");
			Tooltip.SetDefault("Launches homing pumpkins.");
		}

		int counter = 0;
        public override void SetDefaults()
        {
            item.damage = 67;            
            item.melee = true;            
            item.width = 45;              
            item.height = 45;             
            item.useTime = 45;           
            item.useAnimation = 45;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item1;    
          //  item.shoot = mod.ProjectileType("PestilentSwordProjectile");
          //  item.shootSpeed = 4f;
			item.autoReuse = true;			
        }
		 public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			counter++;
			if (counter % 10== 1)
            {
               int newProj = Projectile.NewProjectile(new Vector2(hitbox.X, hitbox.Y), new Vector2(0, 0), mod.ProjectileType("Pumpkin"), item.damage, 0, player.whoAmI);
					Main.projectile[newProj].magic = false;
					Main.projectile[newProj].melee = true;
             
            }
        }
		 public override bool UseItem(Player player)
        {
          // counter = 0;
		   return false;
        }
         public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1729, 12);
            recipe.AddTile(18);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
		}
    }
}