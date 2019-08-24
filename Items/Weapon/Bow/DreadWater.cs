using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class DreadWater : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dread Water");
			Tooltip.SetDefault("Converts arrows into poisonous, murky arrows\nShoots two arrows at once");
		}



        public override void SetDefaults()
        {
            item.damage = 24;
            item.noMelee = true;
            item.ranged = true;
            item.width = 48;
            item.height = 32;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 6;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.crit = 8;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			type = mod.ProjectileType("Dreadshot");
			Projectile.NewProjectile(position.X, position.Y - 10, speedX, speedY, mod.ProjectileType("Dreadshot"), damage, knockBack, player.whoAmI, 0f, 0f);
            return true; 
        }
    }
}