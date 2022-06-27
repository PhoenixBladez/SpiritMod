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
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
    => drawHair = true;
    }
}
