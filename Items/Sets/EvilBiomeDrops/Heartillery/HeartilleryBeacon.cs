using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.EvilBiomeDrops.Heartillery
{
	public class HeartilleryBeacon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aching Heart");
			Tooltip.SetDefault("Summons a stationary heartillery that shoots blood at foes");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.QueenSpiderStaff);
			Item.damage = 22;
			Item.mana = 11;
			Item.width = 20;
			Item.height = 30;
			Item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.knockBack = 2.5f;
			Item.UseSound = SoundID.Item25;
			Item.shoot = ModContent.ProjectileType<HeartilleryMinion>();
			Item.shootSpeed = 0f;
			Item.UseSound = SoundID.Item77;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(Item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY--;
			return !WorldGen.SolidTile(worldX, worldY);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			Projectile.NewProjectile(source, worldX, worldY - pushYUp, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}
	}
}
