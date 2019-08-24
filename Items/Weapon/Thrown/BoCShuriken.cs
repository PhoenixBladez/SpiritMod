using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class BoCShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeping Shuriken");
			Tooltip.SetDefault("Shoots a revolving creeper \n 'Fashioned after yet another fleshy eyeball'");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 14;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("BrainProj");
            item.useAnimation = 24;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 24;
            item.shootSpeed = 15f;
            item.damage = 17;
            item.knockBack = 3.7f;
			item.value = Item.sellPrice(0, 0, 0, 50);
            item.value = Item.buyPrice(0, 0, 0, 60);
            item.rare = 2;
            item.autoReuse = false;
            item.maxStack = 999;
            item.consumable = true;
        }

    }
}
