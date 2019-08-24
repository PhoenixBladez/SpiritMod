using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Skyblade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyblade");
			Tooltip.SetDefault("Swinging the blade can grant you the 'Featherfall' buff");
		}


        public override void SetDefaults()
        {
            item.damage = 28;
            item.melee = true;
            item.width = 54;
            item.height = 58;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 1;
            item.knockBack = 4;
            item.value = Terraria.Item.buyPrice(0, 4, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool UseItem(Player player)
        {
            if (Main.rand.Next(5) == 0)
            {
                player.AddBuff(8, 300);
                return false;

            }
            return false;
        }
         public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 172);
            }
        }
    }
}