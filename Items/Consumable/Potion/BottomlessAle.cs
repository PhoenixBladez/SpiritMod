using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class BottomlessAle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottomless Ale");
			Tooltip.SetDefault("Non-consumable\nMinor improvements to melee stats & lowered defense\n'Down the hatch!'");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = false;
			item.autoReuse = false;

			item.buffType = BuffID.Tipsy;
			item.buffTime = 7200;

			item.UseSound = SoundID.Item3;
		}
	}
}
