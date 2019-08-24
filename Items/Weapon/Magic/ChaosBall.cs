using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Magic
{
	public class ChaosBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowball");
			Tooltip.SetDefault("Shoots an Orb of Chaos.");
		}


		public override void SetDefaults()
		{
			item.damage = 16;
			item.magic = true;
			item.mana = 11;
			item.width = 28;
			item.height = 30;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ChaosBall");
			item.shootSpeed = 14f;
		}
	}
}