using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class EternityCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternity Charm");
			Tooltip.SetDefault("You are the champion of Spirits \n Launches a multitude of Soul Shards when damaged");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 8));
        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.expert = true;
            item.height = 18;
            item.value = Item.buyPrice(0, 22, 0, 0);
            item.rare = 11;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.5f;
			player.maxRunSpeed += 5f;
            player.GetModPlayer<MyPlayer>(mod).OverseerCharm = true;
        }

	}
}
