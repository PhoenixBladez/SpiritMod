using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Spear
{
    public class SanctifiedStabber : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanctified Stabber");
			Tooltip.SetDefault("Inflicts withering leaf");
		}


        public override void SetDefaults()
        {
            item.useStyle = 3;
            item.useTurn = false;
            item.useAnimation = 12;
            item.useTime = 12;
            item.width = 24;
            item.height = 28;
            item.damage = 11;
            item.knockBack = 4f;
            item.scale = 0.9f;
            item.UseSound = SoundID.Item1;
            item.useTurn = true;
            item.value = 3000;
            item.melee = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(mod.BuffType("WitheringLeaf"), 180);
            }
        }
    }
}