using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class ShadowHammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Warhammer");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.value = 10000;
            item.rare = 4;

            item.hammer = 80;

            item.damage = 46;
            item.knockBack = 6;

            item.useStyle = 1;
            item.useTime = 37;
            item.useAnimation = 30;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 109);
            }
        }
    }
}