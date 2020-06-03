using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class StarMap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Map");
			Tooltip.SetDefault("Hold and release to teleport");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.mana = 5;
			item.rare = 5;
			item.width = 28;
			item.height = 30;
			item.useTime = 20;
			item.UseSound = SoundID.Item13;
			item.useStyle = 5;
			item.expert = true;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("StarMapProj");
			item.shootSpeed = 0f;
		}
	}
}
