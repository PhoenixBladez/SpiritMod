using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class HellStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flarespark Staff");
			Tooltip.SetDefault("Summons a geyser of flame");
		}


		public override void SetDefaults()
		{
			item.damage = 24;
			item.magic = true;
			item.mana = 15;
			item.width = 44;
			item.height = 44;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<Firespike>();
			item.shootSpeed = 16f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			Vector2 offset = mouse - player.position;
			offset.Normalize();
			offset *= 60f;
			Projectile.NewProjectile(position + offset, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
