using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitchSet
{
	[AutoloadEquip(EquipType.Head)]
	public class WitchHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charmcaster's Hat");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
	    public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawAltHair = true;
    }
}
