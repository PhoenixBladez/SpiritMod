using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CryoKnife : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Knife");
            Tooltip.SetDefault("Occasionally inflicts 'Cryo Crush'\n'Cryo Crush' does more damage as enemy health wanes\nThis effect does not apply to bosses, and deals a flat amount of damage instead");
        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 30;
            item.height = 30;           
            item.shoot = Projectiles.Thrown.CryoKnife._type;
            item.useAnimation = 19;
            item.useTime = 19;
            item.shootSpeed = 9f;
            item.thrown = true;
            item.damage = 23;
            item.autoReuse = true;
            item.knockBack = 1f;
			item.value = Item.buyPrice(0, 0, 0, 35);
            item.rare = 4;
        }
    }
}
