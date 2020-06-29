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
			item.width = 22;
			item.height = 22;

			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
}
