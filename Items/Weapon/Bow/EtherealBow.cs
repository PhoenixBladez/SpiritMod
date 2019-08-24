using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class EtherealBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ethereal Bow");
            Tooltip.SetDefault("Transforms arrows in to Ethereal Arrows");
        }



        public override void SetDefaults()
        {
            item.damage = 43;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 38;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 6;
            item.value = 1000;
            item.rare = 5;
            item.UseSound = SoundID.Item5;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 10f;
            item.crit = 4;

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("EtherealArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}