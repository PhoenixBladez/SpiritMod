using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.Items.DonatorItems.FrostTroll
{
	public class SnowmongerMountItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowmech");
			Tooltip.SetDefault("Punch");
		}

		public override void SetDefaults()
		{
			Item.width = 23;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.buyPrice(gold: 15);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item106;
			Item.noMelee = true;
			Item.mountType = ModContent.MountType<Mounts.SnowMongerMount.SnowmongerMount>();
		}
	}
}