using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class BabyClamper : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Clamper");
			Tooltip.SetDefault("Increases maximum number of minions by 1\n Increases life regeneration by 3");
		}


        public override void SetDefaults()
        {
            item.width = 24;     
            item.height = 26;   
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.defense = 3;
            item.rare = 3;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 3;
            player.maxMinions += 1;
        }
    }
}
