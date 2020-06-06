using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class CryoSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Sword");
			Tooltip.SetDefault("Occasionally shoots out an icy blast that inflicts 'Cryo Crush'\nCryo Crush deals more damage the less life enemies have left\nThis does not affect bosses, and deals a flat rate of damage instead");
		}

        int charger;
        public override void SetDefaults()
        {
            item.damage = 25;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = Projectiles.CryoliteBlast._type;
            item.shootSpeed = 8;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
                if (Main.rand.Next(4) == 0)
                target.AddBuff(mod.BuffType("CryoCrush"), 300);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 2)
            {
                {
                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack / 2, player.whoAmI, 0f, 0f);

                }
                charger = 0;
            }
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 180);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "CryoliteBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }

    }
}