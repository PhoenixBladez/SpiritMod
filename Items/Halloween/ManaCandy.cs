using SpiritMod.Buffs.Candy;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class ManaCandy : CandyBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Candy");
			Tooltip.SetDefault("Increases mana");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<ManaBuffC>();
			Item.buffTime = 14400;

			Item.UseSound = SoundID.Item2;
		}
	}
}
