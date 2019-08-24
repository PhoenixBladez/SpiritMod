using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CthulhuStaff2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian Wand");
			Tooltip.SetDefault("'Causes otherworldly energy to erupt from the sky'");
		}


        public override void SetDefaults()
        {
            item.damage = 43;
            item.magic = true;
            item.mana = 8;
            item.width = 40;
            item.height = 40;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = Item.sellPrice(0, 7, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("StarfallProjectile");
            item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), mod.ProjectileType("CthulhuBolt"), damage, knockBack, player.whoAmI);
                }
            }
            return false;
        }
    }
}