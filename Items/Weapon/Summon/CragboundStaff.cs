using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class CragboundStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cragbound Staff");
			Tooltip.SetDefault("A tiny Earthen Guardian rains down energy for you \nOccasionally inflicts foes with 'Unstable Affliction'");
		}


		public override void SetDefaults()
		{
			item.height = item.width = 54;
			item.value = Item.sellPrice(0, 8, 45, 0);
			item.rare = ItemRarityID.Cyan;
			item.mana = 20;
			item.damage = 112;
			item.knockBack = 7;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<CragboundMinion>();
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//remove any other owned SpiritBow projectiles, just like any other sentry minion
			for(int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if(p.active && p.type == item.shoot && p.owner == player.whoAmI) {
					p.active = false;
				}
			}
			//projectile spawns at mouse cursor
			Vector2 value18 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = value18;
			return true;
		}
	}
}