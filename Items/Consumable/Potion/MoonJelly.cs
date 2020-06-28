
using SpiritMod.Buffs.Potion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class MoonJelly : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly");
			Tooltip.SetDefault("Regenerates life rapidly");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<MoonBlessing>();
			item.buffTime = 1000;

			item.UseSound = SoundID.Item3;
		}

		public override bool CanUseItem(Player player)
		{
			if(player.FindBuffIndex(BuffID.PotionSickness) >= 0) {
				return false;
			}
			return true;

		}
		public override bool UseItem(Player player)
		{
			if(!player.pStone)
				player.AddBuff(BuffID.PotionSickness, 3600);
			else
				player.AddBuff(BuffID.PotionSickness, 2700);


			return true;
		}
	}
}
