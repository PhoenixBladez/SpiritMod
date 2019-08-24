using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class Apple : CandyBase
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apple");
			Tooltip.SetDefault("'Who the hell gives these out?'");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = -1;
			item.maxStack = 30;
			item.autoReuse = false;
		}
	}
}
