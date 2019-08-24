using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class ScarabBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Bow");
			Tooltip.SetDefault("Converts arrows into Scarab arrows");
		}



        public override void SetDefaults()
        {
            item.damage = 11;
            item.noMelee = true;
            item.ranged = true;
            item.width = 26;
            item.height = 62;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            item.shoot = mod.ProjectileType("ScarabArrow");
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item5;           
            item.autoReuse = true;
            item.shootSpeed = 6.5f;
            item.crit = 8;
        }
		 public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ScarabArrow"), damage, knockBack, player.whoAmI);
            return false;
        }
    }
}