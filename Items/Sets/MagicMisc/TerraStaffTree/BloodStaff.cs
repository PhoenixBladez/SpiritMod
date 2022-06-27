using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
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

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 offset = Vector2.UnitX.RotatedBy(velocity.ToRotation()) * Item.width;
			if (Collision.CanHit(player.Center, 0, 0, player.Center + offset, 0, 0))
				position += offset;
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
