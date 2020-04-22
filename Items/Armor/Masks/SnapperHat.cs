using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
    [AutoloadEquip(EquipType.Head)]
    public class SnapperHat : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapper's Hat");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;

            item.value = 3000;
            item.rare = 1;
            item.vanity = true;
        }
    }
}
