
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
            Item.damage = 5;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = .5f;
            Item.width = 48;
			Item.height = 49;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().rogueCrest = true;
	}
}
