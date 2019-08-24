using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class GigZap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Gigazapper");
			Tooltip.SetDefault("Electrifies your foes");
		}


		public override void SetDefaults()
		{
			item.damage = 88;
            item.thrown = true;
            item.width = 36;
			item.height = 36;
            item.useTime = 17;
			item.useAnimation = 25;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.value = 1009;
			item.useStyle = 1;
			item.knockBack = 5;
			item.rare = 8;
			item.UseSound = SoundID.Item19;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("GigazapperProj");
			item.shootSpeed = 56f;
		}
    }
}
