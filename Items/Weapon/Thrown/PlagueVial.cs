using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class PlagueVial : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Vial");
			Tooltip.SetDefault("A noxious mixture of flammable toxins\nExplodes into cursed embers upon hitting foes");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 16;
            item.height = 16;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item106;
            item.ranged = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("PlagueVialFriendly");
            item.useAnimation = 21;
            item.useTime = 21;
            item.consumable = true;
            item.maxStack = 999;
            item.shootSpeed = 10.0f;
            item.damage = 25;
            item.knockBack = 4.5f;
			item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = 2;
            item.autoReuse = false;
			item.consumable = true;
        }
    }
}