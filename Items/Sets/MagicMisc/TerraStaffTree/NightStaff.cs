using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class NightStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Horizon's Edge");
			Tooltip.SetDefault("Summons a portal at the cursor position that shoots homing energy at enemies\nRight click to cause all portals to explode into a shower of stars\nUp to 2 portals can exist at once");
		}


		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = DamageClass.Magic;
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 20000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item20;
			Item.mana = 14;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<CorruptPortal>();
			Item.shootSpeed = 13f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<CorruptStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<JungleStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DungeonStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HellStaff>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2) {
				{
					for (int projFinder = 0; projFinder < 300; ++projFinder) {
						if (Main.projectile[projFinder].type == type) {
							Main.projectile[projFinder].aiStyle = -3;
							Main.projectile[projFinder].Kill();
						}
					}
				}
				return false;
			}
			else {
				Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
				Projectile.NewProjectile(source, mouse.X, mouse.Y, 0f, 0f, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}