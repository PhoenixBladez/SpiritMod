using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class KakaBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tropic");
			Tooltip.SetDefault("Can also shoot out multiple Dread Arrows or Chlorophyte Arrows");
		}


        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 44;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 40;
            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = 5;
            item.shoot = 9;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 2;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 13f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(8) == 1)
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Dreadshot"), damage, knockBack, player.whoAmI);
                int proj1 = Projectile.NewProjectile(position.X, position.Y - 10, speedX, speedY, mod.ProjectileType("Dreadshot"), damage, knockBack, player.whoAmI);
            }
            else if (Main.rand.Next(8) == 1)
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.ChlorophyteArrow, damage, knockBack, player.whoAmI);
                int proj1 = Projectile.NewProjectile(position.X, position.Y - 10, speedX, speedY, ProjectileID.ChlorophyteArrow, damage, knockBack, player.whoAmI);
            }
            return true;
        }
    }
}