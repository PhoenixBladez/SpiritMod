using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Tenderizer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Tenderizer");
			Tooltip.SetDefault("Tenderizes enemies, allowing you to steal life");
		}


        public override void SetDefaults()
        {
            item.damage = 31;            
            item.melee = true;            
            item.width = 60;              
            item.height = 60;             
            item.useTime = 24;           
            item.useAnimation = 24;     
            item.useStyle = 1;        
            item.knockBack = 5;
            item.rare = 3;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
            item.useTurn = true;
			item.value = Item.sellPrice(0, 2, 0, 0);
            item.crit = 10;
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			if (Main.rand.Next(4) == 1)
			{
            player.HealEffect(4);
            player.statLife += 2;
			}
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 60);
            }
        }
    }
}