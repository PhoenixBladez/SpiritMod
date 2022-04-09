
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class RogueCrest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue's Crest");
			Tooltip.SetDefault("Summons a rusted sword to fight for you\nThis sword does not take up minion slots");
		}

		public override void SetDefaults()
		{
            item.damage = 5;
            item.summon = true;
            item.knockBack = .5f;
            item.width = 48;
			item.height = 49;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 1;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().rogueCrest = true;
	}
}
