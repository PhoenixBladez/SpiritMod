using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DuneHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Helm");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 5;
            item.vanity = true;
        }
    }
}