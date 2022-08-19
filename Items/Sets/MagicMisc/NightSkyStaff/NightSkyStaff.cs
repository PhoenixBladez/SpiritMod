using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.NightSkyStaff
{
	public class NightSkyStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova's Spark");
			Tooltip.SetDefault("Shoots out a fast laser of starry energy\nEvery fifth strike on foes summons a more powerful homing beam of stars\nThat beam rains down homing bolts from the sky");
		}


		int charger;
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 7;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 2.5f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NovaBeam1>();
			Item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			charger++;
			if (charger >= 5) {
				Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X, velocity.Y, ModContent.ProjectileType<NovaBeam2>(), damage / 2 * 3, knockback, player.whoAmI, 0f, 0f);
				charger = 0;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ZephyrBreath.BreathOfTheZephyr>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FrigidSet.HowlingScepter>(), 1);
			recipe.AddIngredient(ModContent.ItemType<GraniteSet.GraniteWand>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BossLoot.StarplateDrops.CosmiliteShard>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
