using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.FrostTroll
{
    public class Chillrend : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chillrend");
			Tooltip.SetDefault("Fires three consecutive rounds of bullets \n Shoots out a homing chilly blast occasionally");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 24;
            item.ranged = true;
            item.width = 58;
            item.height = 32;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 91950;
            item.rare = 6;
            item.UseSound = SoundID.Item31;
            item.autoReuse = true;
            item.shoot = 14;
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Bullet;
            item.crit = 6;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 4)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("FrostBolt"), 64, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return true;
        }
    }
}