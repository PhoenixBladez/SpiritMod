using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GhastSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghastblade");
            Tooltip.SetDefault("Shoots out a ghast wisp that may deal multiple frames of damage\nIf the wave does not ignore enemy immunity frames, it inflicts 'Wisp Wrath' upon them");
        }


        public override void SetDefaults()
        {
            item.width = 52;
            item.height = 52;
            item.rare = 8;
            item.damage = 72;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 9, 0, 0);
            item.useStyle = 1;
            item.useTime = item.useAnimation = 17;
            item.melee = true;
            item.shoot = mod.ProjectileType("GhastWisp");
            item.shootSpeed = 9f;
            item.autoReuse = true;
            item.UseSound = SoundID.Item66;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;

        }
    }
}