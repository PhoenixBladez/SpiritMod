using SpiritMod.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.InfernonDrops
{
	public class DiabolicHorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diabolic Horn");
			Tooltip.SetDefault("Provides a fiery platform to fly on");
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.noMelee = true;

			Item.mountType = ModContent.MountType<DiabolicPlatform>();

			Item.UseSound = SoundID.Item25;
		}
	}
}
