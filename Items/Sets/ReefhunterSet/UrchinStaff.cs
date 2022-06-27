using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class UrchinStaff : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin Lobber");

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.width = 28;
			Item.height = 14;
			Item.useTime = Item.useAnimation = 30;
			Item.reuseDelay = 2; //Prevent item from being reused while projectile is still active, making projectile bug out
			Item.knockBack = 2f;
			Item.shootSpeed = 8f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<UrchinStaffProjectile>();
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 16);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 targetPos = Main.MouseWorld;
			float minTargetRadius = 150f;
			if (player.Distance(targetPos) <= minTargetRadius) //Doesn't instantly drop or move backwards
				targetPos = player.Center + player.DirectionTo(targetPos) * minTargetRadius;

			Projectile proj = Projectile.NewProjectileDirect(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI);
			if(proj.ModProjectile is UrchinStaffProjectile staffProj)
			{
				staffProj.TargetPosition = targetPos - player.MountedCenter;
				if (Main.netMode != NetmodeID.SinglePlayer) //sync extra ai as projectile is made
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			}
			return false;
		}
	}
}