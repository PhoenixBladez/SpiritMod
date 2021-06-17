using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts.Minecarts.MarbleMinecart
{
	public class MarbleMinecartItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nemean Chariot");
			Tooltip.SetDefault("When charging, you gain defense.\n\"A good lookin' ride, to be sure\" - Professor Temp. O. Rary");

			MountID.Sets.Cart[item.mountType] = true;
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 25000;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item69; //nice
			item.noMelee = true;
			item.mountType = ModContent.MountType<MarbleMinecart>();
		}

		public override bool CanUseItem(Player player) => false; //the player shouldn't be able to use this item but they can so that's cool I guess don't worry
	}
}