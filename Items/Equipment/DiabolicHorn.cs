using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
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
			item.width = 22;
			item.height = 20;
            item.value = 10000;
            item.rare = 5;

            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = mod.MountType("DiabolicPlatform");

            item.UseSound = SoundID.Item25;
        }
	}
}
