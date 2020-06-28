using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class FallenAngel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Sigil");
			Tooltip.SetDefault("Magic attacks may fire an angelic spark\nThis spark deals more damage the less mana the player has left");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.defense = 1;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{

			player.GetSpiritPlayer().manaWings = true;
		}

	}
}
