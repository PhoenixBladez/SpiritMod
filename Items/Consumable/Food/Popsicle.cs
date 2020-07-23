
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;

namespace SpiritMod.Items.Consumable.Food
{
	public class Popsicle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Popsicle");
			Tooltip.SetDefault("Minor improvements to all stats\nGrants immunity to 'On Fire'\n'Eat it before it melts!'");
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
			item.buffTime = 36000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<IceBerryBuff>(), 7200);
			return true;
		}
	}
}
