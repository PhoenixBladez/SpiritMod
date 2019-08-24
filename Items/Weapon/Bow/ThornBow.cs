using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class ThornBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornshot");
			Tooltip.SetDefault("Arrows shot may split into multiple damaging thorns upon hitting an enemy\nThorns may poison foes");
		}



        public override void SetDefaults()
        {
            item.damage = 21;
            item.noMelee = true;
            item.ranged = true;
            item.width = 22;
            item.height = 56;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 2;
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.value = Item.sellPrice(0, 2, 30, 0);
            item.autoReuse = true;
            item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromThornBow = true;
            return false;
        }
    }
}