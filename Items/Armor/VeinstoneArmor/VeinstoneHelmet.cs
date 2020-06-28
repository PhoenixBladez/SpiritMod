using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.VeinstoneArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class VeinstoneHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Veinstone Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;

			item.vanity = true;
		}
	}
}
