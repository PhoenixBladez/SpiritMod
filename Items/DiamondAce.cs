using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items
{
	public class DiamondAce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Ace");
			Tooltip.SetDefault("You shouldn't see this");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 5));
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 1;
		}
	}
}
