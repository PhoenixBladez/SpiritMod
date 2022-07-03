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

			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 3000;
			Item.rare = ItemRarityID.Cyan;
		}
	}
}
