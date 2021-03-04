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
	public class ClubMealThree : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Meal");
			Tooltip.SetDefault("You shouldn't see this");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
		}

		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange = 0;
		}
		public override bool OnPickup(Player player)
		{
			player.AddBuff(BuffID.WellFed, 120);
			Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 139, 2);
			return false;
		}
	}
}
