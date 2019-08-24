using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class StarlightBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Spray");
			Tooltip.SetDefault("Let it rain Stars!");
		}



        public override void SetDefaults()
        {
            item.damage = 16;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 40;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            item.shoot = 9;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shootSpeed = 20f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 9, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}