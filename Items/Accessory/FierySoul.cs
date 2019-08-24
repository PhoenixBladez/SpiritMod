using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class FierySoul : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Soul");
			Tooltip.SetDefault("Getting hurt releases embers \n Minions have a chance to burn enemies");
		}


        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.rare = 5;
            item.defense = 2;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void UpdateEquip(Player player)
        {
            ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).Fierysoul = true;
        }
    }
}
