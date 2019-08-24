using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class ShadowflameSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Sword");
            Tooltip.SetDefault("Causes explosions of Shadowflames to appear when hitting enemies\nShoots out Shadow Embers that damage nearby foes");

        }


        public override void SetDefaults()
        {
            item.width = item.height = 42;
            item.rare = 6;
            item.damage = 44;
            item.knockBack = 6;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 20;
            item.melee = true;
            item.shoot = mod.ProjectileType("ShadowPulse1");
            item.shootSpeed = 2;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.autoReuse = true;
            item.UseSound = SoundID.Item33;   
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dist = 80;
                Vector2 targetExplosionPos = target.Center;
                for (int i = 0; i < 200; ++i)
                {
                    if (Main.npc[i].active && (Main.npc[i].Center - targetExplosionPos).Length() < dist)
                    {
                        Main.npc[i].HitEffect(0, damage);
                    }
                }
                for (int i = 0; i < 15; ++i)
                {
                    int newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[newDust].noGravity = true;
                    Main.dust[newDust].velocity *= 5f;
                    newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                }
            }
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(BuffID.ShadowFlame, 300, true);
            }
        }
    }
}