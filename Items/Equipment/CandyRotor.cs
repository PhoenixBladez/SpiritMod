using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	class CandyRotor : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Candy Rotor");
            Tooltip.SetDefault("Ride a Helicopter to Victory!\n...a rather tiny one.");
        }
        public override void SetDefaults()
		{
			item.width = 16;
			item.height = 22;
            item.value = 50000;
            item.rare = 8;

            item.useStyle = 4;
			item.useTime = 20;
            item.useAnimation = 20;

            item.noMelee = true;

			item.mountType = mod.MountType("CandyCopter");

            item.UseSound = SoundID.Item25; //Find a better sound
        }		
	}
}
