using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
    public class EoWSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rot Scourge");
            Tooltip.SetDefault("Hitting foes may cause them to release multiple tiny, homing eaters");
        }


        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.noMelee = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.shootSpeed = 5f;
            item.knockBack = 3f;
            item.damage = 20;
            item.value = Item.sellPrice(0, 1, 15, 0);
            item.rare = 2;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("EoWSpearProj");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

    }
}
