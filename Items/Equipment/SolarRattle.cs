using System;

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
			Tooltip.SetDefault("Summons a Drakomire into battle \n When riding the Drakomire, defense is increased by 40 \n A fiery trail is left behind and knockback is ignored \n The Drakomire also builds up stamina, allowing for a dash every 10 seconds.");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = 9;

			item.useStyle = 1;
			item.useTime = 20;
			item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = mod.MountType("Drakomire");

			item.UseSound = SoundID.Item25;
		}
	}
}
