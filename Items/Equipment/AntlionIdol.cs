using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class AntlionIdol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Idol");
			Tooltip.SetDefault("Summons a rideable Antlion Charger\nThe Charger increases your mining speed by 20% when underground");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = 800;
            item.rare = 2;

            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;

			item.noMelee = true;

			item.mountType = mod.MountType("AntlionMount");

            item.UseSound = SoundID.Item25;
        }
	}
}
