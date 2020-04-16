using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp Staff");
			Tooltip.SetDefault("Shoots out a beam of energy that strikes enemies multiple times\nThis energy may split into a pulse of shadows");
		}


		public override void SetDefaults()
		{
			item.damage = 45;
			item.magic = true;
			item.mana = 8;
			item.width = 50;
			item.height = 50;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 4;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item92;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ExplodingWispShadow");
			item.shootSpeed = 13.5f;
		}
	}
}
