using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CrystalShadow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Shadow");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 14;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("CrystalShadow");
            item.useAnimation = 22;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 22;
            item.shootSpeed = 12f;
            item.damage = 45;
            item.knockBack = 3f;
			item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 5;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
			item.crit = 4;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			  int newProj = Projectile.NewProjectile(position.X, position.Y,  speedX, speedY, type, damage, knockBack, player.whoAmI);
			  Main.projectile[newProj].hostile = false;
                Main.projectile[newProj].friendly = true;
			return false;
		}
    }
}
