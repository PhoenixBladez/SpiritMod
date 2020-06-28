using SpiritMod.Buffs.Candy;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class ManaCandy : CandyBase
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Candy");
			Tooltip.SetDefault("Increases mana");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Green;
			item.maxStack = 30;

			item.useStyle = 2;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<ManaBuffC>();
			item.buffTime = 14400;

			item.UseSound = SoundID.Item2;
		}
	}
}
