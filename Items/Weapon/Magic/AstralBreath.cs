using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AstralBreath : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Breath");
			Tooltip.SetDefault("Shoots magical dragonbreath");
		}


		public override void SetDefaults()
		{
			item.damage = 34;
			item.magic = true;
			item.mana = 11;
			item.width = 44;
			item.height = 44;
			item.useTime = 5;
			item.useAnimation = 20;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("AstralFlare");
			item.shootSpeed = 2f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int I = 0; I < 2; I++)
            {
                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 100), speedY + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}