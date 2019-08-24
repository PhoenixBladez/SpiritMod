using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class ShadowAxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Battleaxe");
		}


        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 46;
            item.value = 10000;
            item.rare = 4;

            item.axe = 16;

            item.damage = 39;
            item.knockBack = 6;

            item.useStyle = 1;
            item.useTime = 35;
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