using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class UrchinStaff : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin Lobber");

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.reuseDelay = 2; //Prevent item from being reused while projectile is still active, making projectile bug out
			item.knockBack = 2f;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.magic = true;
			item.mana = 10;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(gold: 2);
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<UrchinStaffProjectile>();
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 16);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 targetPos = Main.MouseWorld;
			float minTargetRadius = 150f;
			if (player.Distance(targetPos) <= minTargetRadius) //Doesn't instantly drop or move backwards
				targetPos = player.Center + player.DirectionTo(targetPos) * minTargetRadius;

			Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			if(proj.modProjectile is UrchinStaffProjectile staffProj)
			{
				staffProj.TargetPosition = targetPos - player.MountedCenter;
				if (Main.netMode != NetmodeID.SinglePlayer) //sync extra ai as projectile is made
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			}
			return false;
		}
	}
}