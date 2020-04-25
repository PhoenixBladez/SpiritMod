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
            item.useTime = 100;
            item.useAnimation = 100;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("RightHopper");
            item.shootSpeed = 12f;
        }
		   public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (!player.HasBuff(BuffID.Featherfall))
			{
			player.velocity.X = 0 - speedX;
			player.velocity.Y = 0 - speedY;
			}
			
			return false;
		}
		
	}
}