using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.PigronStaff
{
	public class PigronStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pigron Staff");
			Tooltip.SetDefault("'Bacon now fights for you'");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.mana = 12;
			item.damage = 29;
			item.knockBack = 2;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<PigronMinion>();
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}