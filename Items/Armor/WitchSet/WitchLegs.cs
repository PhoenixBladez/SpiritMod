using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitchSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class WitchLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charmcaster's Leggings");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
    }
}
