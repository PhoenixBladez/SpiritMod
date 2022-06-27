using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OperativeSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class OperativeLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Operative's Speedpants");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;

			Item.vanity = true;
		}
    }
}
