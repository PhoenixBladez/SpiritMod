using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class BladeOfNoah : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade of Noah");
            Tooltip.SetDefault("Penetrates 5 times \n~Donator Item~");

        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 26;
            item.height = 26;
            item.shoot = mod.ProjectileType("BladeOfNoah");
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 10f;
            item.damage = 45;
            item.knockBack = 1.0f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 5);
            item.rare = 5;
            item.autoReuse = true;
        }

      
    }
}
