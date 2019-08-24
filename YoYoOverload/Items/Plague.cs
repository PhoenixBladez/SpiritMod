using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Plague : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague");
			Tooltip.SetDefault("You really, REALLY want to let go...");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 15;
			base.item.value = 10150;
			base.item.rare = 2;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(3) == 0)
			{
				type = base.mod.ProjectileType("PlagueT");
				return true;
			}
			type = base.mod.ProjectileType("PlagueP");
			return true;
		}
	}
}
