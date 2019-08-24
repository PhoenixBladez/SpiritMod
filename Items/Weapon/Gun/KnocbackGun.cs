using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class KnocbackGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Cannon");
			Tooltip.SetDefault("Right click to shoot out slower, extremely powerful bullets");
		}

		
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 65;
            item.height = 21;
            item.useTime = 20;
            item.useAnimation = 20;
			item.useTurn = false;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = Item.buyPrice(0, 11, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
			player.itemAnimation = 1;
			return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
				player.itemTime += 50;
				player.itemAnimation += 50;
				player.itemAnimationMax += 50;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 242, (int)(damage * 1.6f), knockBack + 10f, player.whoAmI);
            }
            else
            {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}