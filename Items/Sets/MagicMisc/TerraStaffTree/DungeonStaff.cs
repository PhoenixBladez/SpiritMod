using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class DungeonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skullspray Staff");
			Tooltip.SetDefault("Summons a skull at the cursor position\nThe skull erupts into four bolts of energy");
		}


		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 14;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 0, 8, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<AquaSphere>();
			Item.shootSpeed = 13f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				Vector2 mouse = Main.MouseWorld;
				Projectile.NewProjectile(source, mouse.X, mouse.Y, 0, 0, ModContent.ProjectileType<AquaSphere>(), damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
