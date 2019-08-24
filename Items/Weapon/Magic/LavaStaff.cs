using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class LavaStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lavacloud Staff");
			Tooltip.SetDefault("A smoky cloud guards you, throwing fire at your curor's position");
		}


		public override void SetDefaults()
		{
			item.damage = 49;
			item.magic = true;
			item.mana = 18;
			item.width = 40;
			item.height = 40;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("LavaCloud");
			item.shootSpeed = 12f;
		}
	}
}
