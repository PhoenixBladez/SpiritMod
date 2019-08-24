using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class AlphaBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Blade");
			Tooltip.SetDefault("'The power of the universe sides with you'");
		}


        public override void SetDefaults()
        {
            item.damage = 200;            
            item.melee = true;            
            item.width = 70;              
            item.height = 76;             
            item.useTime = 16;           
            item.useAnimation = 16;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(1, 0, 0, 0);
            item.rare = 12;
            item.UseSound = SoundID.Item1;    
            item.shoot = mod.ProjectileType("PestilentSwordProjectile");
            item.shootSpeed = 4f;
			item.autoReuse = true;			
        }
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Vector2 origVect = new Vector2(speedX, speedY);
			//generate the remaining projectiles
			
			Vector2 newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj1"), damage, knockBack, player.whoAmI, 0f, 0f);
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj2"), damage, knockBack, player.whoAmI, 0f, 0f);
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj3"), damage, knockBack, player.whoAmI, 0f, 0f);
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj4"), damage, knockBack, player.whoAmI, 0f, 0f);
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, newVect.X * 1.5f, newVect.Y * 1.5f, mod.ProjectileType("AlphaProj5"), damage, knockBack, player.whoAmI, 0f, 0f);
				
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EternityEssence", 20);
            recipe.AddIngredient(null, "SpiritStar", 1);
            recipe.AddIngredient(3467, 10);
            recipe.AddIngredient(3456, 4);
            recipe.AddIngredient(3457, 4);
            recipe.AddIngredient(3458, 4);
            recipe.AddIngredient(3459, 4);
            recipe.AddTile(412);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}