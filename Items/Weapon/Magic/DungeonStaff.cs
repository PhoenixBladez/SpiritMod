using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class DungeonStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Spray");
			Tooltip.SetDefault("Shoots a splitting bolt of aqua energy");
		}


        public override void SetDefaults()
        {
            item.damage = 20;
            item.magic = true;
            item.mana = 14;
            item.width = 42;
            item.height = 42;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("AquaSphere");
            item.shootSpeed = 13f;
        }
    }
}
