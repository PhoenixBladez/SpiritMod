using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
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
			item.damage = 34;
			item.magic = true;
			item.mana = 7;
			item.width = 58;
			item.height = 58;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 2.5f;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<NovaBeam1>();
			item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			charger++;
			if (charger >= 5) {
				Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX, speedY, ModContent.ProjectileType<NovaBeam2>(), damage / 2 * 3, knockBack, player.whoAmI, 0f, 0f);
				charger = 0;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BreathOfTheZephyr>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ClapdateStaff>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HowlingScepter>(), 1);
			recipe.AddIngredient(ModContent.ItemType<GraniteWand>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
