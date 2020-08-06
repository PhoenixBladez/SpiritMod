using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ProtectorateSet
{
	[AutoloadEquip(EquipType.Body)]
	public class ProtectorateBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Protectorate Uniform");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
