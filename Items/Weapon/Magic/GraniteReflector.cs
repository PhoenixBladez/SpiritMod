using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GraniteReflector : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Reflector");
			Tooltip.SetDefault("Summons a wall of energy");
		}


		public override void SetDefaults()
		{
			item.damage = 44;
			item.magic = true;
			item.mana = 32;
			item.width = 40;
			item.height = 40;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 12;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 7, 0, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("GraniteWall");
			item.shootSpeed = 120f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.type == item.shoot && p.owner == player.whoAmI) 
				{
					p.active = false;
				}
			}
			if (speedX > 0)
			{
				position.X = position.X + 108;
			}
			if (speedX <= 0)
			{
				position.X = position.X - 108;
			}
			position.Y = position.Y - 30;
			speedX *= 0.001f;
			speedY = 0;
			return true;
        }
	}
}