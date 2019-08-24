using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Eyeshot : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eyeshot");
			Tooltip.SetDefault("Arrows turn into Eyeballs!");
		}



        public override void SetDefaults()
        {
            item.damage = 14;
            item.noMelee = true;
            item.ranged = true;
            item.width = 50;
            item.height = 30;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 6f;
        }
            public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("EyeArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;

        }
    }
}
