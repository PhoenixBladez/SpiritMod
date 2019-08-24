using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class HealthCandy : CandyBase
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Health Candy");
			Tooltip.SetDefault("Increases Health");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = 2;
			item.maxStack = 30;

			item.useStyle = 2;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = mod.BuffType("HealthBuffC");
			item.buffTime = 14400;

			item.UseSound = SoundID.Item2;
		}
	}
}
