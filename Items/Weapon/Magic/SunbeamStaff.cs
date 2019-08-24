using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class SunbeamStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunbeam Staff");
            Tooltip.SetDefault("Shoots out a fast moving, homing solar bolt");
        }



        public override void SetDefaults()
        {
            item.damage = 18;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 23;
            item.mana = 8;
            item.useAnimation = 23;
            item.useStyle = 5;
            item.knockBack = 3f ;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shootSpeed = 14;
            item.shoot = mod.ProjectileType("SolarBeamFriendly");
        }
    }
}
