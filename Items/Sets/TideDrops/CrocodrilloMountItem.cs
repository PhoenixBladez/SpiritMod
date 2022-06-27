using SpiritMod.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops
{
	public class CrocodrilloMountItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bag O' Bait");
			Tooltip.SetDefault("Summons a friendly Crocodrillo mount");
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.noMelee = true;

			Item.mountType = ModContent.MountType<TideMount>();

			Item.UseSound = SoundID.Item25;
		}
	}
}
