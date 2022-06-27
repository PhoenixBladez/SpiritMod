using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using SpiritMod.Buffs;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Sets.ClubSubclass.ClubSandwich
{
	public class ClubMealOne : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Meal");
			Tooltip.SetDefault("You shouldn't see this");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override bool OnPickup(Player player)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 2));
			{
				player.AddBuff(BuffID.WellFed, 240);
			}
			return false;
		}
	}
}
