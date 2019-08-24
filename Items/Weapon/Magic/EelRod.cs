using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Items.Weapon.Magic
{
	public class EelRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eel Tail");
			Tooltip.SetDefault("Shoots a delayed spurt of electrical energy");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;			
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 2;
			item.damage = 21;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 26;
			item.useAnimation = 29;
			item.mana = 6;
            item.knockBack = 3;
			item.magic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("EelOrb");
			item.shootSpeed = 15f;
		}

    }
}
