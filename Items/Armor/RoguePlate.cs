using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class RoguePlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Plate");

			ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Terraria.Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
	}
}