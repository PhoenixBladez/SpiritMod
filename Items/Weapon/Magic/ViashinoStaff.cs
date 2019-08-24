using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ViashinoStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viashino Wand");
			Tooltip.SetDefault("'It hails from a far off, fiery plane'");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.buyPrice(0, 8, 0, 0);
			item.rare = 5;
			item.damage = 44;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 36;
			item.useAnimation = 36;
			item.mana = 14;
			item.magic = true;
            item.autoReuse = false;
			item.noMelee = true;
			item.shoot = 666;
			item.shootSpeed = 10f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 666, damage, knockBack, player.whoAmI);
                Vector2 mouse = Main.MouseWorld;
                Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), 666, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
