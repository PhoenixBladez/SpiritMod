using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class SevenSins : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Seven Sins");
			Tooltip.SetDefault("Occasionally shoots out a fire lash the combusts foes.");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 44;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 38;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1;
            item.rare = 5;
            item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 2, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 13f;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                {
                    charger++;
                    if (charger >= 5)
                    {
                        for (int I = 0; I < 1; I++)
                        {
                            Projectile.NewProjectile(position.X, position.Y, speedX * 4, speedY * 4, mod.ProjectileType("FireSin"), 50, knockBack, player.whoAmI, 0f, 0f);
                        }
                        charger = 0;
                    }
                    return true;
                }
            }
        }
    }
}