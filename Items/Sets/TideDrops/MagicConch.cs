using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops
{
	public class MagicConch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shankya");
			Tooltip.SetDefault("Summons a whirlpool at the location of the cursor");
		}


		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 0f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MagicConchProj>();
			Item.shootSpeed = 0f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouse = Main.MouseWorld;
			Terraria.Projectile.NewProjectile(source, mouse.X, mouse.Y, 0f, 0f, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}
