using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FreemanSet
{
	[AutoloadEquip(EquipType.Head)]
	public class FreemanHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Freeman's Goggles");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
    => drawHair = true;
    }
}
