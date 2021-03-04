using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using SpiritMod.Buffs;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items
{
	public class ClubMealFour : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Meal");
			Tooltip.SetDefault("You shouldn't see this");
		}
		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 8;
			item.maxStack = 1;
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override bool OnPickup(Player player)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 2));
			{
				player.AddBuff(BuffID.WellFed, 540);
			}
			return false;
		}
	}
}
