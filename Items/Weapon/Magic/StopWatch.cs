using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class StopWatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Clock");
			Tooltip.SetDefault("Creates a pulse around the player, stopping time. \nHas a 60 second cooldown");
		}


		public override void SetDefaults()
		{
			item.damage = 33;
			item.magic = true;
			item.mana = 100;
			item.width = 40;
			item.height = 40;
			item.useTime = 100;
			item.useAnimation = 100;
			item.useStyle = 5;
			Item.staff[item.type] = false; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("Shockwave");
			item.shootSpeed = 0.3f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			 MyPlayer modPlayer = player.GetSpiritPlayer();
            modPlayer.shootDelay = 3600;
			float spread = 10f * 0.0174f;
			double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
			double deltaAngle = spread / 8f;
			for (int i = 0; i < 36; i++)
			{
				double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
				Projectile.NewProjectile(position.X, position.Y, (float)(Math.Sin(offsetAngle) * 15f), (float)(Math.Cos(offsetAngle) * 15f), mod.ProjectileType("WatchPulse"), damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, (float)(-Math.Sin(offsetAngle) * 15f), (float)(-Math.Cos(offsetAngle) * 15f), mod.ProjectileType("WatchPulse"), damage, knockBack, player.whoAmI);
			}
			speedX = 0;
			speedY = 0;
			return true;
		}
		public override bool CanUseItem(Player player)
        {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (modPlayer.shootDelay == 0)
                return true;
            return false;
        }
	}
}
