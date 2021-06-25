using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class ReachMask : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane Mask");
		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
}
