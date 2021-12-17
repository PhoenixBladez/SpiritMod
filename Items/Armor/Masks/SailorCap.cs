using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class SailorCap : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain's Cap");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;

			item.value = 500;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
}
