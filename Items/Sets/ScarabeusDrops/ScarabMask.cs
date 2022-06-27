using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class ScarabMask : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarabeus Mask");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;

			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			//drawHair = true;
			drawAltHair = true;
		}
	}
}
