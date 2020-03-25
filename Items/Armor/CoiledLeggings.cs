using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CoiledLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autonaut's Leggings");

        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
            item.rare = 2;
            item.vanity = true;
        }
    }
}
