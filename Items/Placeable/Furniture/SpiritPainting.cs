using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SpiritPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Milestone");
		}


		public override void SetDefaults()
		{
            item.width = 94;
			item.height = 62;
            item.value = item.value = Terraria.Item.buyPrice(0, 10, 1000, 10);
            item.rare = 6;

            item.maxStack = 99;

            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("SpiritPainting");
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "'Quite a milestone indeed!\nThings can only look up from here'");
            line.overrideColor = new Color(50, 50, 230);
            tooltips.Add(line);
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                }
            }
        }
    }
}