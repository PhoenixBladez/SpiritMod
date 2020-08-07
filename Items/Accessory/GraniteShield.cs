using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class GraniteShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Shield");
			Tooltip.SetDefault("Grants you Shadow Dodge when under 50 health\nRecharges when above 150 health");
		}


		private bool Meme;
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 12, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.defense = 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife < 50 && Meme) {
				Meme = false;
				player.AddBuff(59, 220);
			}
			if (player.statLife > 150) {
				Meme = true;
			}
		}
	}
}
