using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class HellsGaze : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Lash");
			Tooltip.SetDefault("Nearby enemies are engulfed by fire \n Increases critical strike chance by 6% \n You emit a fiery glow");
		}



        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.rare = 4;
            item.value = 80000;
            item.expert = true;
            item.melee = true;
            item.accessory = true;

            item.knockBack = 9f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).HellGaze = true;
            player.meleeCrit += 6;
            player.rangedCrit += 6;
            player.magicCrit += 6;
            player.thrownCrit += 6;
        }
    }
}
