using SpiritMod.Buffs.Candy;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class Taffy : CandyBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Taffy");
			Tooltip.SetDefault("Increases defense");
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

			Item.buffType = ModContent.BuffType<TaffyBuff>();
			Item.buffTime = 14400;

			Item.UseSound = SoundID.Item2;
		}
	}
}
