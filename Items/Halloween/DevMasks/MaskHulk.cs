using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.DevMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class MaskHulk : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hulk Mask");
            Tooltip.SetDefault("Vanity item");

        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 9;
        }
    }
}
