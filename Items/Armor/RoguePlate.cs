using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class RoguePlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Plate");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 18;
			item.value = Terraria.Item.buyPrice(0, 0, 20, 0);
			item.rare = 1;
			item.defense = 2;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
	}
}