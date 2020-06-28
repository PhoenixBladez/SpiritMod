using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
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
			item.damage = 20;
			item.magic = true;
			item.mana = 14;
			item.width = 42;
			item.height = 42;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = Terraria.Item.sellPrice(0, 0, 8, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<AquaSphere>();
			item.shootSpeed = 13f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			{
				if(Main.myPlayer == player.whoAmI) {
					Vector2 mouse = Main.MouseWorld;
					Projectile.NewProjectile(mouse.X, mouse.Y, 0, 0, ModContent.ProjectileType<AquaSphere>(), damage, knockBack, player.whoAmI);
				}
			}
			return false;
		}
	}
}
