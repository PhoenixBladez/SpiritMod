using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Equipment
{
	public class DynastyFan : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Dynasty Fan"); 
			Tooltip.SetDefault("Launch yourself");
		}

		 public override void SetDefaults()
        {
            item.width = 44;
            item.height = 48;
            item.useTime = 120;
            item.useAnimation = 120;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("RightHopper");
            item.shootSpeed = 14f;
        }
		   public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			player.velocity.X = 0 - speedX;
			player.velocity.Y = 0 - speedY;
			if (player.HasBuff(8))
			{
				player.velocity.X *= 0.2f;
				player.velocity.Y *= 0.2f;
			}
			return false;
		}
		
	}
}