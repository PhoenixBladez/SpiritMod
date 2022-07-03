using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class CimmerianScepter : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cimmerian Scepter");
			Tooltip.SetDefault("Summons a magic scepter to fight for you\nThe scepter uses a multitude of attacks against foes\nThis scepter does not take up minion slots");
		}

		public override void SetDefaults()
		{
            Item.damage = 22;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 1.5f;
            Item.width = 22;
			Item.height = 40;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
	}
}
