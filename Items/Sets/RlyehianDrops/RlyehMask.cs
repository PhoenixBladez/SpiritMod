using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RlyehianDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class RlyehMask : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian Mask");
		}

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
