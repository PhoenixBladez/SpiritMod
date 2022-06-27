using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HunterArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SnowRangerLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fur Legwraps");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
	}
}
