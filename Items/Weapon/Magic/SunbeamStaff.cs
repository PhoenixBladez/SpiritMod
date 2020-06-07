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
			DisplayName.SetDefault("Photosynthestrike");
            Tooltip.SetDefault("Shoots out a fast moving, homing solar bolt");
        }



        public override void SetDefaults()
        {
            item.damage = 19;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 19;
            item.mana = 8;
            item.useAnimation = 19;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 0f;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shootSpeed = 6;
            item.shoot = ModContent.ProjectileType<SolarBeamFriendly>();
        }
    }
}
