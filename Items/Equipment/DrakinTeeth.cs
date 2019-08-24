using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class DrakinTeeth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakin Teeth");
			Tooltip.SetDefault("Summons a Drakin to ride upon!\nIncreases damage dealt by 5% and defense by 10\nMagic attacks may shoot out Drakin Breath along with regular projectiles");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = 10000;
            item.rare = 7;

            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = mod.MountType("DrakinMount");

            item.UseSound = SoundID.Item25;
        }
	}
}
