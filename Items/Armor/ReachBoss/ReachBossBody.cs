using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
	[AutoloadEquip(EquipType.Body)]
	public class ReachBossBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Garb");

		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 30200;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
