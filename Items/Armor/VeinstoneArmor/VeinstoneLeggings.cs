using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.VeinstoneArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class VeinstoneLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Veinstone Leggings");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;

            item.vanity = true;
        }
    }
}
