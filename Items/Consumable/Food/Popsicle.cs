
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
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.buffType = BuffID.WellFed;
			Item.buffTime = 36000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<IceBerryBuff>(), 7200);
			return true;
		}
	}
}
