using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class InfernonMask : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Infernon Mask");


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
