using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AstralLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Lens");
			Tooltip.SetDefault("Shoots out bursts of electrical stars \n 'Scry the stars and let them work in your favor'");
		}


		public override void SetDefaults()
		{
			item.damage = 29;
			item.magic = true;
			item.mana = 9;
			item.width = 44;
			item.height = 44;
			item.useTime = 29;
			item.useAnimation = 29;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 3;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("Starshock1");
			item.shootSpeed = 8f;
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