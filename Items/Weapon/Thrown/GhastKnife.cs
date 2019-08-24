using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class GhastKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wispedge");
            Tooltip.SetDefault("Pierces through walls and ignores gravity\nInflicts 'Wisp Wrath' and has a chance to deal multiple frames of damage");
                }


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 32;
            item.height = 18;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item60;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("GhastKnife");
            item.useAnimation = 13;
            item.consumable = false;
            item.maxStack = 1;
            item.useTime = 13;
            item.shootSpeed = 11f;
            item.damage = 56;
            item.knockBack = 2.5f;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 20);
            item.rare = 8;
            item.autoReuse = true;
        }
    }
}