using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FreemanSet
{
	[AutoloadEquip(EquipType.Body)]
	public class FreemanBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Freeman's Platemail");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
    }
}
