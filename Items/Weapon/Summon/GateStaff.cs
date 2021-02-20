using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class GateStaff : ModItem
	{
		readonly static int righthopper = ModContent.ProjectileType<RightHopper>();
		readonly static int lefthopper = ModContent.ProjectileType<LeftHopper>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gate Staff");
			Tooltip.SetDefault("Left click and right click to summon an electric field");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.summon = true;
			item.mana = 16;
			item.width = 44;
			item.height = 48;
			item.useTime = 55;
			item.useAnimation = 55;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 20000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<RightHopper>();
			item.shootSpeed = 0f;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = (player.altFunctionUse == 2) ? righthopper : lefthopper;
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld);
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray);
			dist = 0;
			foreach (float array in scanarray) {
				dist += array / (scanarray.Length);
			}
			Vector2 spawnpos = player.Center + player.DirectionTo(Main.MouseWorld) * dist;
			Projectile.NewProjectileDirect(spawnpos, Vector2.Zero, type, damage, knockBack, player.whoAmI, 0, -1);
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.statMana <= 12) {
				return false;
			}
			if (player.altFunctionUse == 2) {
				if(player.ownedProjectileCounts[righthopper] > 0) {
					foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == player.whoAmI && x.type == righthopper))
						proj.Kill();
				}
			}
			else {
				if (player.ownedProjectileCounts[lefthopper] > 0) {
					foreach (Projectile proj in Main.projectile.Where(x => x.active && x.owner == player.whoAmI && x.type == lefthopper))
						proj.Kill();
				}
			}
			return true;
		}
	}
}