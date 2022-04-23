using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class GoldenApple : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Apple");
			Tooltip.SetDefault("Increases defense as health decreases");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 12, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.defense = 2;
			item.accessory = true;
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			if (!player.HasAccessory<GoldShield>() || !player.HasAccessory<MedusaShield>())
			{
				float defBoost = ((float)player.statLifeMax2 - player.statLife) / player.statLifeMax2 * 15f;
				player.statDefense += (int)defBoost;
			}
		}
	}
}
