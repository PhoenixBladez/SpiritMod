using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Pins
{
	public class BlueMapPin : ModItem
	{
		//TODO: Minimap integration, saving upon world exit, make them not draw in corner when not in use
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Blue Pin"); 
			Tooltip.SetDefault("Places a map pin at your location \nRight Click to delete pin");
		}

		 public override void SetDefaults()
        {
           
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 2;
            item.autoReuse = false;
            item.shootSpeed = 0f;
        }
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override bool UseItem(Player player)
		{
			Main.PlaySound(0, (int)player.position.X, (int)player.position.Y);
			if (player.altFunctionUse != 2)
			{
				((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).BluePinX = (int)(player.position.X / 16);
				((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).BluePinY = (int)(player.position.Y / 16);
			}
			else
			{
				((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).BluePinX = 0;
				((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).BluePinY = 0;
			}
			return true;
		}
	}
}