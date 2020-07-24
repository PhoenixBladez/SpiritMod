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
		public override bool ItemSpace(Player player) => true;
		public override bool OnPickup(Player player)
		{
			player.AddBuff(ModContent.BuffType<AceOfDiamondsBuff>(), 180);
			Main.PlaySound(7, (int)player.position.X, (int)player.position.Y);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, 100);
		}
	}
}
