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
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 1;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = false;
			Item.autoReuse = false;

			Item.buffType = BuffID.Tipsy;
			Item.buffTime = 7200;

			Item.UseSound = SoundID.Item3;
		}
	}
}
