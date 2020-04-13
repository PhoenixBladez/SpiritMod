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
			Tooltip.SetDefault("Occasionally inflicts Ichor\nKilling enemies with this weapon heals life");
		}


        public override void SetDefaults()
        {
            item.damage = 26;            
            item.melee = true;            
            item.width = 60;              
            item.height = 60;             
            item.useTime = 23;           
            item.useAnimation = 23;     
            item.useStyle = 1;        
            item.knockBack = 3;
            item.rare = 3;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
            item.useTurn = true;
			item.value = Item.sellPrice(0, 1, 50, 0);
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(6) == 0)
            {
                target.AddBuff(BuffID.Ichor, 240);
            }
			if (target.life <= 0)
			{
                int healNumber = Main.rand.Next(5, 8);
                player.HealEffect(healNumber);
                if (player.statLife <= player.statLifeMax - healNumber)
                player.statLife += healNumber;
                else
                player.statLife += player.statLifeMax - healNumber;
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}