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
			item.width = 22;
			item.height = 20;
			item.value = 30200;
			item.rare = ItemRarityID.Green;
			item.vanity = true;
		}
	}
}
