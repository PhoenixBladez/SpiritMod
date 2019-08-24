using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Wings)]
    public class FlierWings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone King's Wings");
			Tooltip.SetDefault("Allows for flight and slow fall \n You are the king of the skies");
		}

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 60000;
            item.rare = 3;
            item.expert = true;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 51;
		}

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
    ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
			ascentWhenFalling = 0.69f;
			ascentWhenRising = 0.7f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1.5f;
			constantAscend = 0.12f;
		}

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 5f;
			acceleration *= 1.2f;
		}  
    }
}