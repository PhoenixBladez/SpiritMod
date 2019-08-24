using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class FoundryGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Blaster");
		}


        public override void SetDefaults()
        {
            item.damage = 45;
            item.ranged = true;
            item.width = 52;       
            item.height = 24;      
            item.useTime = 20;  
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 2.4f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item45;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BouncingMagma");
            item.shootSpeed = 14f;
        }
		        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0174532925);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 3);
            for (int i = 0; i < 2; ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ThermiteBar", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
