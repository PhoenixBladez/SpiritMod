using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;


namespace SpiritMod.Items.Weapon.Swung
{
    public class TimeWinder : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time Winder");
			Tooltip.SetDefault("It's not about how much time you have, it's about how you use it.");
		}


        public override void SetDefaults()
        {
            item.damage = 130;
            item.melee = true;
            item.width = 35;
            item.height = 35;
            item.useTime = 10;
            item.useAnimation = 16;
            item.useStyle = 1;
            item.knockBack = 10;
            item.value = 16000;
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            for (int i = 0; i < 3; i++)
            {
                Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), .4f, .9f, 1f);
            }
        }

        public override void OnHitNPC(Player p, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(2) == 1)
            {
                Vector2 velocity = new Vector2(p.direction, 0) * 4f;
                int proj = Terraria.Projectile.NewProjectile(p.Center.X, p.position.Y + p.height + -35, velocity.X, velocity.Y, mod.ProjectileType("TimeWinderClone"), damage = 60, item.owner, 0, 0f);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
            }
        }
    }
}