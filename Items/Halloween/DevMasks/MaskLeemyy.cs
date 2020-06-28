using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.DevMasks
{
	[AutoloadEquip(EquipType.Head)]
	public class MaskLeemyy : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leemyy's Mask");
			Tooltip.SetDefault("Vanity item \n'Great for impersonating devs!'");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = ItemRarityID.Cyan;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = true;
		}
	}
}
