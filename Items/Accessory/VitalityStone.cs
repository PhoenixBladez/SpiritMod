using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class VitalityStone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vitality Stone");
			Tooltip.SetDefault("Increases life regeneration and invincibility time slightly \n 'The night is dark and full of terrors'");
		}


        public override void SetDefaults()
        {
            item.width = 48;     
            item.height = 49;   
            item.value = Item.sellPrice(0, 0, 56, 0);
            item.rare = 2;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 3;   
        }
    }
}
