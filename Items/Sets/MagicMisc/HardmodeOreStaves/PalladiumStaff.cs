using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class PalladiumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Staff");
			Tooltip.SetDefault("Summons a runic pillar at the cursor position\nIf below 1/3 HP, step inside the pillar to rapidly regenerate health");
		}


		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 15;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item83;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PalladiumStaffProj>();
			Item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if (p.active && p.type == Item.shoot && p.owner == player.whoAmI) {
					p.active = false;
				}
			}
			Vector2 mouse = Main.MouseWorld;
			Terraria.Projectile.NewProjectile(source, mouse.X, mouse.Y, 0f, 100f, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.PalladiumBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
