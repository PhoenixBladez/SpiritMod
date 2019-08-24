using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class QuicksilverKunai : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Kunai");
            Tooltip.SetDefault("Shoots out three bouncing Kunai");
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
            item.shoot = mod.ProjectileType("QuicksilverKunai");
            item.useAnimation = 19;
            item.useTime = 19;
            item.shootSpeed = 8.5f;
            item.damage = 58;
            item.knockBack = 3.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.crit = 8;
            item.rare = 8;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Kunai_Throwing", 33);
            recipe.AddIngredient(null, "Material", 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 66);
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
			 downX = (float)(0 - (Math.Sin(angledown) * 9.5));
			 downY = (float)(0 - (Math.Cos(angledown) * 9.5));
			   upX = (float)(0 - (Math.Sin(angleup) * 9.5));
			  upY = (float)(0 - (Math.Cos(angleup) * 9.5));
			 }
			 
			 if (SdirY > 0)
			 {
			  downX = (float)(Math.Sin(angledown) * 9.5);
			  downY = (float)(Math.Cos(angledown) * 9.5);
			  upX = (float)(Math.Sin(angleup) * 9.5);
			  upY = (float)(Math.Cos(angleup) * 9.5);
			 }
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("QuicksilverKunai"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position.X, position.Y, downX, downY, mod.ProjectileType("QuicksilverKunai"), damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position.X, position.Y, upX, upY, mod.ProjectileType("QuicksilverKunai"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
