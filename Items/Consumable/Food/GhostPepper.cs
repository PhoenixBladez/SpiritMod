
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class GhostPepper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Pepper");
			Tooltip.SetDefault("Minor improvements to all stats\n'Will you take the risk?'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 18000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item3;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(116, 5400);
            player.AddBuff(24, 420);
            return true;
		}
	}
}
