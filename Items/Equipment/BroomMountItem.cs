using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class BroomMountItem : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Broom");
			Tooltip.SetDefault("Ride a mythical witch's broom\nRaises magic and summon damage by 8%");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = 10000;
            item.rare = 4;

            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = Mounts.BroomMount._type;

            item.UseSound = SoundID.Item25;
        }
	}
}
