using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class StrangeKelp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Kelp");
			Tooltip.SetDefault("Summons a Greenfin Treader for you to mount!");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = 10000;
            item.rare = 3;

            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = mod.MountType("TideMount");

            item.UseSound = SoundID.Item25;
        }
	}
}
