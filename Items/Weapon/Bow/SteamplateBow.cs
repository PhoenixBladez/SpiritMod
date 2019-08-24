using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class SteamplateBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcharger");
			Tooltip.SetDefault("Converts arrows into Starcharged Arrows");
		}



        public override void SetDefaults()
        {
            item.damage = 32;
            item.noMelee = true;
            item.ranged = true;
            item.width = 48;
            item.height = 32;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 0, 40, 0);
            item.autoReuse = true;
            item.shootSpeed = 7f;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SteamArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false; 
        }
    }
}