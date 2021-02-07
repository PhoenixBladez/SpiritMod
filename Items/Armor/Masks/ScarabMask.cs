using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
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
			item.width = 22;
			item.height = 20;

			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			//drawHair = true;
			drawAltHair = true;
		}
	}
}
