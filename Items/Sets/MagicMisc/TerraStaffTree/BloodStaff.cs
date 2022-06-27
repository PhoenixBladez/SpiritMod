using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class BloodStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vessel Drainer");
			Tooltip.SetDefault("Summons a blood tentacle that splits into life-stealing blood clots");
		}


		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 44;
			Item.useAnimation = 44;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.crit += 10;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BloodVessel>();
			Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 offset = Vector2.UnitX.RotatedBy(new Vector2(speedX, speedY).ToRotation()) * Item.width;
			if (Collision.CanHit(player.Center, 0, 0, player.Center + offset, 0, 0))
				position += offset;

			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<CrimsonStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<JungleStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DungeonStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HellStaff>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.Register();
		}

	}
}
