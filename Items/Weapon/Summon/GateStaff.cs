using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class GateStaff : ModItem
	{
		int RightHopper => ModContent.ProjectileType<RightHopper>();
		int LeftHopper => ModContent.ProjectileType<LeftHopper>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gate Staff");
			Tooltip.SetDefault("Left click and right click to summon an electric field");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 16;
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 55;
			Item.useAnimation = 55;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<RightHopper>();
			Item.shootSpeed = 0f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			type = (player.altFunctionUse == 2) ? RightHopper : LeftHopper;
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld);
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray);
			dist = 0;

			foreach (float array in scanarray)
				dist += array / (scanarray.Length);

			Vector2 spawnpos = player.Center + player.DirectionTo(Main.MouseWorld) * dist;
			Projectile.NewProjectileDirect(source, spawnpos, Vector2.Zero, type, damage, knockback, player.whoAmI, 0, -1);
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana <= 12)
				return false;

			if (player.altFunctionUse == 2)
			{
				if (player.ownedProjectileCounts[RightHopper] > 0)
				{
					foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == player.whoAmI && x.type == RightHopper))
						proj.Kill();
				}
			}
			else
			{
				if (player.ownedProjectileCounts[LeftHopper] > 0)
				{
					foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == player.whoAmI && x.type == LeftHopper))
						proj.Kill();
				}
			}
			return true;
		}
	}
}