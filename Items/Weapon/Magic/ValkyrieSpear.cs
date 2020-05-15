using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ValkyrieSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Spirit Spear");
			Tooltip.SetDefault("Deals both magic and melee damage");
		}


		public override void SetDefaults()
		{
			item.damage = 16;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ValkyrieSpearHostile");
			item.shootSpeed = 11.5f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            for (int I = 0; I < 2; I++)
			{
                int p = Projectile.NewProjectile(position.X, position.Y, speedX + ((float) Main.rand.Next(-250, 250) / 100), speedY - ((float) Main.rand.Next(100, 250) / -100), type, damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
                Main.projectile[p].melee = true;
                Main.projectile[p].magic = true;
            }
			return false;
		}
	}
}
