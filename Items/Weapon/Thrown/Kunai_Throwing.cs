using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Kunai_Throwing : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kunai");
		}


		float downX;
		float downY;
		float upX;
		float upY;
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("Kunai_Throwing");
            item.useAnimation = 20;
            item.useTime = 20;
            item.shootSpeed = 7.5f;
            item.damage = 12;
            item.knockBack = 3.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 1, 0);
            item.crit = 8;
            item.rare = 2;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddTile(16);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
	    recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddIngredient(ItemID.LeadBar, 2);
            recipe.AddTile(16);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            float SdirX = (float)(Main.MouseWorld.X - player.position.X);
            float SdirY = (float)(Main.MouseWorld.Y - player.position.Y);
            float angleup = (float)Math.Atan(SdirX / SdirY) + 25;
			 float angledown = (float)Math.Atan(SdirX / SdirY) - 25;
			 if (SdirY < 0)
			 {
			 downX = (float)(0 - (Math.Sin(angledown) * 8.5));
			 downY = (float)(0 - (Math.Cos(angledown) * 8.5));
			   upX = (float)(0 - (Math.Sin(angleup) * 8.5));
			  upY = (float)(0 - (Math.Cos(angleup) * 8.5));
			 }
			 
			 if (SdirY > 0)
			 {
			  downX = (float)(Math.Sin(angledown) * 8.5);
			  downY = (float)(Math.Cos(angledown) * 8.5);
			  upX = (float)(Math.Sin(angleup) * 8.5);
			  upY = (float)(Math.Cos(angleup) * 8.5);
			 }
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position.X, position.Y, downX, downY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position.X, position.Y, upX, upY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
