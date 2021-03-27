
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
			Tooltip.SetDefault("Summons an possessed bow to fight for you\nUses arrows for ammunition\nAutomatically converts fired arrows into the strongest ammo type in your inventory");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 55, 0);
			item.rare = 2;
			item.damage = 16;
			item.knockBack = 2;
			item.summon = true;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetSpiritPlayer().bowSummon = true;
		}
	}
}
