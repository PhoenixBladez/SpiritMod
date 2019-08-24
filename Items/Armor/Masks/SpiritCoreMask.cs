using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritCoreMask : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Umbra Mask");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;

            item.value = 3000;
            item.rare = 1;
            item.vanity = true;
        }
    }
}
