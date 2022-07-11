using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.BowSummonItem
{
	public class BowSummonItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxbow");
			Tooltip.SetDefault("Summons a possessed bow to fight for you\nUses the strongest arrows in your inventory\nIf the user has no arrows, shoots weaker Jester Arrows infinitely");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 16;
			Item.knockBack = 2;
			Item.DamageType = DamageClass.Summon;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().bowSummon = true;
	}
}
