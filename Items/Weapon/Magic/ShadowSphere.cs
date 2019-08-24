using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowSphere : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Sphere");
			Tooltip.SetDefault("Summons a slow shadow sphere that shoots out Crystal Shadows at foes");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = 5;
			item.damage = 39;
			item.useStyle = 5;
			item.UseSound = SoundID.Item20;
			Item.staff[item.type] = true;
			item.useTime = 36;
			item.useAnimation = 36;
			item.mana = 10;
			item.summon = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ShadowCircleRune");
			item.shootSpeed = 0f;
		}
		        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //remove any other owned SpiritBow projectiles, just like any other sentry minion
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
                {
                    p.active = false;
                }
            }
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
	}
}
