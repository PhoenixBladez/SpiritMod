using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class FolvBlade1 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Lost Blade");
			Tooltip.SetDefault("Replenishes mana upon hitting enemies\n~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 14;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = 1;
            item.autoReuse = false;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.crit = 6;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                Main.dust[d].noGravity = true;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.statMana += 5;
        }
    }
}