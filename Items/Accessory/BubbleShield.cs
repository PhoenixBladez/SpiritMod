using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class BubbleShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Shield");
			Tooltip.SetDefault("Cloaks you in a bubble of invincibility upon taking fatal damage. Consumable");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;

            item.defense = 3;
            item.rare = 8;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).bubbleShield = true;
        }
    }
}
