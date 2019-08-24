using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class FireBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Sphere");
			Tooltip.SetDefault("Shoots a Blazing Orb of Fire");
		}


		public override void SetDefaults()
		{
			item.damage = 19;
			item.magic = true;
			item.mana = 11;
			item.width = 28;
			item.height = 30;
			item.useTime = 24;
			item.useAnimation = 30;
			item.useStyle = 5;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
            item.shoot = 376;
            item.shootSpeed = 12f;
		}
	}
}