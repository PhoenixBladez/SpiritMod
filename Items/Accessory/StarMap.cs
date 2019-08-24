using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class StarMap : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Map");
			Tooltip.SetDefault("Increases movement speed by 10% and critical strike chance by 4% \n Getting hurt spawns stars from the sky \n 'Let the stars guide you'");
		}


        public override void SetDefaults()
        {
            item.width = 34;     
            item.height = 56;   
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.defense = 2;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>(mod).starMap = true;
            player.maxRunSpeed += .1f;
            player.meleeCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.rangedCrit += 4;
        }

    }
}
