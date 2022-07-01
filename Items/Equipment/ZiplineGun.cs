using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.Zipline;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Equipment
{
	public class ZiplineGun : ModItem
	{
		bool leftactive = false;
		int distance = 500;
		bool rightactive = false;
		int right = 0;
		int left = 0;

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rail-gun");
			Tooltip.SetDefault("Left and right click to shoot tethers that latch to tiles\nThese tethers are connected by a rail\nHold UP to slide down the rail \nDoes not work with steep rails");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Ranged;
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
			Item.shoot = ModContent.ProjectileType<RightZipline>();
			Item.shootSpeed = 16.7f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/MaliwanShot1"), position);
			if ((rightactive && Main.projectile[right].active == false) || (leftactive && Main.projectile[left].active == false)) //if the gates despawn, reset
			{
				Main.projectile[right].active = false;
				Main.projectile[left].active = false;
				rightactive = false;
				leftactive = false;
				right = 0;
				left = 0;
			}

			if (player.altFunctionUse == 2) {
				if (rightactive) {
					Main.projectile[right].active = false;
				}
				right = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<RightZipline>(), Item.damage, 1, Main.myPlayer);
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
				left = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<LeftZipline>(), Item.damage, 1, Main.myPlayer);
				leftactive = true;
				if (rightactive) {
					Main.projectile[left].ai[1] = right;
					Main.projectile[right].ai[1] = left;
				}
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.MinecartTrack, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
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