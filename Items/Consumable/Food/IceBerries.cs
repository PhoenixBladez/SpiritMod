using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;

namespace SpiritMod.Items.Consumable.Food
{
	public class IceBerries : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Berry");
			Tooltip.SetDefault("Grants immunity to being on fire");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = 2;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = ModContent.BuffType<IceBerryBuff>();
			item.buffTime = 19600;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
	}
}
