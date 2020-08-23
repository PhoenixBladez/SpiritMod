
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class CimmerianScepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cimmerian Scepter");
			Tooltip.SetDefault("Summons a magic scepter to fight for you\nThe scepter uses a multitude of attacks against foes\nThis scepter does not take up minion slots");
		}


		public override void SetDefaults()
		{
            item.damage = 22;
            item.summon = true;
            item.knockBack = 1.5f;
            item.width = 22;
			item.height = 40;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = 3;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetSpiritPlayer().cimmerianScepter = true;
		}
	}
}
