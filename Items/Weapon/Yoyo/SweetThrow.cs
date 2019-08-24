using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Yoyo
{
	public class SweetThrow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sweet Throw");
			Tooltip.SetDefault("Releases bees to chase down your foes");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 25;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.knockBack = 2;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 25;
            item.useTime = 27;
            item.shoot = mod.ProjectileType("SweetThrowProjectile");           
        }
    }
}
