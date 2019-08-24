using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Rune1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Spellbinder");
			Tooltip.SetDefault("Shoots out a barrage of runes");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 53;
			base.item.value = 200000;
			base.item.rare = 7;
			base.item.knockBack = 1f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 28;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("RuneP");
		}
	}
}
