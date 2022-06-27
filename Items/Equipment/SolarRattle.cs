using SpiritMod.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class SolarRattle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Rattle");
			Tooltip.SetDefault("Summons a Drakomire into battle\nWhen riding the Drakomire, defense is increased by 40\nA fiery trail is left behind and knockback is ignored\nThe Drakomire also builds up stamina, allowing for a dash every 10 seconds.");
		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 30, 0, 0);
			Item.rare = ItemRarityID.Cyan;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.noMelee = true;

			Item.mountType = ModContent.MountType<Drakomire>();

			Item.UseSound = SoundID.Item25;
		}
	}
}
