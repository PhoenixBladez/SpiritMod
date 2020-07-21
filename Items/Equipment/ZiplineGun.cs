using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Summon.Zipline;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class ZiplineGun : ModItem
	{
		bool leftactive = false;
		int distance = 500;
		bool rightactive = false;
		int right = 0;
		int left = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rail-gun");
			Tooltip.SetDefault("Left and right click to shoot tethers that latch to tiles\nThese tethers are connected by a rail\nHold UP to slide down the rail \nDoes not work with steep rails");
		}

		public override void SetDefaults()
		{
			item.ranged = true;
			item.width = 44;
			item.height = 48;
			item.useTime = 55;
			item.useAnimation = 55;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 20000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<RightHopper>();
			item.shootSpeed = 13f;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MaliwanShot1"));
			if ((rightactive && Main.projectile[right].active == false) || (leftactive && Main.projectile[left].active == false)) //if the gates despawn, reset
			{
				Main.projectile[right].active = false;
				Main.projectile[left].active = false;
				rightactive = false;
				leftactive = false;
				right = 0;
				left = 0;
			}
			/*  if (player.statMana <= 12)
              {
                  return false;
              }*/
			if (player.altFunctionUse == 2) {
				if (rightactive) {
					Main.projectile[right].active = false;
				}
				right = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<RightZipline>(), item.damage, 1, Main.myPlayer);
				rightactive = true;
				if (leftactive) {
					Main.projectile[right].ai[1] = left;
					Main.projectile[left].ai[1] = right;
				}
			}
			else {
				if (leftactive) {
					Main.projectile[left].active = false;
				}
				left = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<LeftZipline>(), item.damage, 1, Main.myPlayer);
				leftactive = true;
				if (rightactive) {
					Main.projectile[left].ai[1] = right;
					Main.projectile[right].ai[1] = left;
				}
			}
			return false;
		}
		public override ModItem Clone(Item item)
		{
			ZiplineGun staff = (ZiplineGun)base.Clone(item);
			staff.left = left;
			staff.right = right;
			staff.distance = distance;
			staff.rightactive = rightactive;
			staff.leftactive = leftactive;
			return staff;
		}
	}
}