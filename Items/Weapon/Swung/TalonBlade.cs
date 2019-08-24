using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class TalonBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Blade");
			Tooltip.SetDefault("Launches fossilized feathers");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 27;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;             
            item.useTime = 30;           
            item.useAnimation = 30;     
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("BoneFeatherFriendly");
            item.shootSpeed = 10f;            
            item.crit = 8;  
			item.autoReuse = true;
        }
      
		 public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        { 
                {
                    charger++;
                    if (charger >= 5)
                    {
                        for (int I = 0; I < 1; I++)
                        {
                            Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("GiantFeather"), damage, knockBack, player.whoAmI, 0f, 0f);
                        }
                        charger = 0;
                    }
                    return true;
                }
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

