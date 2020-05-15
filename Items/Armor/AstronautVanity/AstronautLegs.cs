using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.AstronautVanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class AstronautLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astronaut Pants");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
            item.rare = 2;
            item.vanity = true;
        }
    }
}
