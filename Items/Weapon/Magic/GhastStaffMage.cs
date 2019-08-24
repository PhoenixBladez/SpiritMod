using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GhastStaffMage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Dragon");
			Tooltip.SetDefault("Rains down a Spirit Dragon that may deal multiple frames of damage\nEnemies directly hit by the Spirit Dragon combust into homing wisps");
		}


        public override void SetDefaults()
        {
            item.damage = 61;
            item.magic = true;
            item.mana = 8;
            item.width = 50;
            item.height = 50;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = 5;
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("StarfallProjectile");
            item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 1; ++i)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Projectile.NewProjectile(mouse.X, player.Center.Y - 570 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), mod.ProjectileType("GhastDragon"), damage, knockBack, player.whoAmI);
                }
            }
            return false;
        }
    }
}