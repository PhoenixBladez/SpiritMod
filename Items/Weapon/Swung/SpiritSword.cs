using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class SpiritSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Blade");
			Tooltip.SetDefault("Swings occasionally spawn a Spirit Aura to damage all nearby enemies");
		}


		public override void SetDefaults()
		{
			item.damage = 45;
			item.useTime = 29;
			item.useAnimation = 29;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 5;
			item.shootSpeed = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.crit = 6;
			item.shoot = mod.ProjectileType("GeodeStaveProjectile");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 1; I++)
			{
				if (Main.rand.Next(3) == 0)
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SpiritAura"), 30, knockBack, player.whoAmI);
				}
			}
			return false;
		}

	}
}
