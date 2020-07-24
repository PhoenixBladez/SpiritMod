using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CowboySet
{
	[AutoloadEquip(EquipType.Body)]
	public class CowboyBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw's Vest");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
	}
}
