using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SwiftRune : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swiftness Rune");
			Tooltip.SetDefault("Gives your Shuriken a Boost!\nIncreases thrown velocity and movement speed");
		}


        public override void SetDefaults()
        {
            item.width = 42;     
            item.height = 42;   
            item.value = Item.sellPrice(0, 0, 66, 0);
            item.rare = 2;
            
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.thrownVelocity += 0.12f;
            player.maxRunSpeed += 0.09f;
        }
    }
}
