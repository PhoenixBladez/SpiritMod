using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class SnapperHat : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapper's Hat");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;

			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
