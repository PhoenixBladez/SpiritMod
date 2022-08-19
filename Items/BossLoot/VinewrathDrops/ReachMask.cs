using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.VinewrathDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class ReachMask : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane Mask");
		}


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
