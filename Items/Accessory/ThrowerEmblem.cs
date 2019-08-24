using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class ThrowerEmblem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Emblem");
			Tooltip.SetDefault("Increases throwing damage by 15%");
		}


        public override void SetDefaults()
        {
            item.width = 48;     
            item.height = 49;   
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 4;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.thrownDamage += 0.15f;        
        }
    }
}
