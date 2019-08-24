using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class BlackTide : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Tide");
			Tooltip.SetDefault("Calls forth a spread of Black Sludge that lingers in place");
		}


        public override void SetDefaults()
        {
            item.damage = 21;
            item.magic = true;
            item.mana = 8;
            item.width = 46;
            item.height = 46;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item34;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Blacksludge");
            item.shootSpeed = 11f;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int I = 0; I < 2; I++)
            {
                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 100), speedY + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
