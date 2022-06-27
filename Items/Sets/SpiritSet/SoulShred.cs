
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SoulShred : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Ember");
			Tooltip.SetDefault("'A part of the everburning Soul'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 6));
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Pink;
			Item.width = 14;
			Item.maxStack = 99;
			Item.height = 36;
		}
	}
}