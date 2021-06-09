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
	public class CrimsonSkull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Skull");
			Tooltip.SetDefault("You shouldn't see this");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 7));
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 32;
			item.maxStack = 1;
			item.alpha = 50;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override bool ItemSpace(Player player) => true;
		public override void GrabRange(Player player, ref int grabRange) => grabRange = 0;

		public override bool OnPickup(Player player)
		{
			player.AddBuff(mod.BuffType("CrimsonSkullBuff"), 240);
			Main.PlaySound(42, (int)player.position.X, (int)player.position.Y, 139, 1f, -0.9f);
			return false;
		}
	}
}
