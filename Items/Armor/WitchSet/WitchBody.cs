using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitchSet
{
	[AutoloadEquip(EquipType.Body)]
	public class WitchBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charmcaster's Robe");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
    }
}
