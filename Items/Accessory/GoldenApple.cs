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
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 2;
			Item.accessory = true;
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
