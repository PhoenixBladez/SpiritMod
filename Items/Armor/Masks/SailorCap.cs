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
			Item.width = 22;
			Item.height = 22;

			Item.value = 500;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
