using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class GateStaff : ModItem
	{
		bool leftactive = false;
		Vector2 direction9 = Vector2.Zero;
		int distance = 500;
		bool rightactive = false;
		int right = 0;
		int left = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gate Staff");
			Tooltip.SetDefault("Left click and right click to summon an electric field");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
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
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return false;
		}
		public override ModItem Clone(Item item)
		{
			GateStaff staff = (GateStaff)base.Clone(item);
			staff.left = this.left;
			staff.right = this.right;
			staff.distance = this.distance;
			staff.rightactive = this.rightactive;
			staff.leftactive = this.leftactive;
			return staff;
		}
		public override bool CanUseItem(Player player)
		{
			if((rightactive && Main.projectile[right].active == false) || (leftactive && Main.projectile[left].active == false)) //if the gates despawn, reset
			{
				Main.projectile[right].active = false;
				Main.projectile[left].active = false;
				rightactive = false;
				leftactive = false;
				right = 0;
				left = 0;
			}
			if(player.statMana <= 12) {
				return false;
			}
			if(player.altFunctionUse == 2) {
				if(rightactive) {
					Main.projectile[right].active = false;
				}
				right = Projectile.NewProjectile((int)(Main.screenPosition.X + Main.mouseX), (int)(Main.screenPosition.Y + Main.mouseY), 0, 0, ModContent.ProjectileType<RightHopper>(), item.damage, 1, Main.myPlayer);
				rightactive = true;
				if(leftactive) {
					Main.projectile[right].ai[1] = left;
					Main.projectile[left].ai[1] = right;
					direction9 = Main.projectile[right].Center - Main.projectile[left].Center;
					distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					if(distance < 500) {
						Main.PlaySound(SoundID.Item93, player.position);
					}
				}
			} else {
				if(leftactive) {
					Main.projectile[left].active = false;
				}
				left = Projectile.NewProjectile((int)(Main.screenPosition.X + Main.mouseX), (int)(Main.screenPosition.Y + Main.mouseY), 0, 0, ModContent.ProjectileType<LeftHopper>(), item.damage, 1, Main.myPlayer);
				leftactive = true;
				if(rightactive) {
					Main.projectile[left].ai[1] = right;
					Main.projectile[right].ai[1] = left;
					direction9 = Main.projectile[right].Center - Main.projectile[left].Center;
					distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					if(distance < 500) {
						Main.PlaySound(SoundID.Item93, player.position);
					}
				}
			}
			return true;
		}
	}
}