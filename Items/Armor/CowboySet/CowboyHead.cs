using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CowboySet
{
	[AutoloadEquip(EquipType.Head)]
	public class CowboyHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw's Hat");

			ArmorIDs.Head.Sets.UseAltFaceHeadDraw[Item.headSlot] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
    }
}
