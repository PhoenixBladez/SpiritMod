using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
	public class Sovereign_Talon : ModItem
	{
		public int charger = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sovereign Talon");
			Tooltip.SetDefault("Thrusting continuously charges the talon\nAn arcane wave will be cast at full charge");
		}

		public override void SetDefaults()
		{
			item.damage = 33;
			item.useStyle = 5;
			item.useAnimation = 18;
			item.useTime = 18;
			item.shootSpeed = 2.7f;
			item.knockBack = 2.5f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = 4;
			item.value = Item.sellPrice(gold: 2, silver: 50);
			item.melee = true;
			item.noMelee = true;
			item.channel = true;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("Sovereign_Talon_Projectile");
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("Sovereign_Talon_Projectile_Mirrored")] < 1 && player.ownedProjectileCounts[mod.ProjectileType("Sovereign_Talon_Projectile")] < 1;
		}
		
		public override void HoldItem (Player player)	
		{
			if (!player.channel)
				charger = 0;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.direction != -1)
			{
				type = mod.ProjectileType("Sovereign_Talon_Projectile_Mirrored");
			}
			else
			{
				type = mod.ProjectileType("Sovereign_Talon_Projectile");
			}
			charger++;
			if (charger < 11)
			{
				int p = Projectile.NewProjectile(position.X, position.Y, speedX*Main.rand.Next(-2,2)*player.direction, speedY*5f*player.direction*Main.rand.Next(-1,1), mod.ProjectileType("Visual_Talon"), 0, 0f, player.whoAmI);
				Main.projectile[p].timeLeft = 9999;
			}
			else
			{
				Main.PlaySound(2, (int)position.X, (int)position.Y, 73, 1f, 0f);
				charger = 0;
				int p = Projectile.NewProjectile(position.X, position.Y, speedX*8f, speedY*8f, mod.ProjectileType("Talon_Projectile"), damage*2, knockBack*2f, player.whoAmI);
			}
				
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}
