using Microsoft.Xna.Framework;
using SpiritMod.Items.BossLoot.DuskingDrops;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class DarkstarLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkstar Lantern");
			Tooltip.SetDefault("Creates a singularity at the cursor position that shoots out multiple void stars\nVoid stars home in on enemies and may inflict Shadowflame");

		}


		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 66;
			Item.height = 68;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.crit = 10;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item93;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<ShadowOrb>();
			Item.shootSpeed = 1f;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<DuskStone>(), 10);
			modRecipe.AddIngredient(ItemID.Ectoplasm, 14);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouse = Main.MouseWorld;
			Terraria.Projectile.NewProjectile(source, mouse.X, mouse.Y, 0f, 0f, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}