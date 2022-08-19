using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.InfernonDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class InfernonMask : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Infernon Mask");


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;

			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
