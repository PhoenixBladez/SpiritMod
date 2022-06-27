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

			MountID.Sets.Cart[Item.mountType] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 25000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item69; //nice
			Item.noMelee = true;
			Item.mountType = ModContent.MountType<MarbleMinecart>();
		}

		public override bool CanUseItem(Player player) => false; //the player shouldn't be able to use this item but they can so that's cool I guess don't worry
	}
}