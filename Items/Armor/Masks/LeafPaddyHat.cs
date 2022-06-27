using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class LeafPaddyHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rice Paddy Hat");
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;

			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
    }
}
