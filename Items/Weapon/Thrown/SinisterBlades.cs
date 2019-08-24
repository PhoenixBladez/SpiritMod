using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SinisterBlades : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sinister Blades");
			Tooltip.SetDefault("Shoots 2 blades on use");
		}


        float downX;
        float downY;
        float upX;
        float upY;
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 22;
            item.height = 22;
			item.autoReuse = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SinisterBladeProj");
            item.useAnimation = 17;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 17;
            item.shootSpeed = 8.0f;
            item.damage = 51;
            item.knockBack = 7f;
			item.value = Terraria.Item.sellPrice(0, 0, 4, 0);
            item.rare = 8;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
            item.crit = 13;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float SdirX = (float)(Main.MouseWorld.X - player.position.X);
            float SdirY = (float)(Main.MouseWorld.Y - player.position.Y);
            float angleup = (float)Math.Atan(SdirX / SdirY) + 25;
            float angledown = (float)Math.Atan(SdirX / SdirY) - 25;
            if (SdirY <= 0)
            {
                downX = (float)(0 - (Math.Sin(angledown) * 10.5));
                downY = (float)(0 - (Math.Cos(angledown) * 10.5));
                upX = (float)(0 - (Math.Sin(angleup) * 10.5));
                upY = (float)(0 - (Math.Cos(angleup) * 10.5));
            }

            if (SdirY > 0)
            {
                downX = (float)(Math.Sin(angledown) * 10.5);
                downY = (float)(Math.Cos(angledown) * 10.5);
                upX = (float)(Math.Sin(angleup) * 10.5);
                upY = (float)(Math.Cos(angleup) * 10.5);
            }
            Terraria.Projectile.NewProjectile(position.X, position.Y, downX, downY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            Terraria.Projectile.NewProjectile(position.X, position.Y, upX, upY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightmareFuel", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 33);
            recipe.AddRecipe();
        }

    }
}
