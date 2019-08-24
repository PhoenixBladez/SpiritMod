using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
    [AutoloadEquip(EquipType.Head)]
    public class IlluminantMask : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Master Mask");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;

            item.value = 3000;
            item.rare = 1;
            item.vanity = true;
        }
    }
}
