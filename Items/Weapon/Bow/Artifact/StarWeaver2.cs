using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow.Artifact;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow.Artifact
{
	public class StarWeaver2 : ModItem
	{
		int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			Tooltip.SetDefault("Converts arrows into two Astral Bolts\nAstral Bolts may split into five damaging shards of energy\nRight click to shoot out an explosive Burning Core every second\nHold right-click to increase the power of Burning Cores, resetting at three");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.noMelee = true;
			item.ranged = true;
			item.width = 28;
			item.height = 66;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1.5f;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 50);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.useTurn = false;
			item.shootSpeed = 8f;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2) {

				MyPlayer modPlayer = player.GetSpiritPlayer();
				modPlayer.shootDelay = 60;
				charger++;
				if (charger >= 1) {
					{
						Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Stars1"), 40, 4, player.whoAmI, 0f, 0f);
					}
				}
				if (charger >= 2) {
					{
						Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Stars2"), 70, 5, player.whoAmI, 0f, 0f);
					}
				}
				if (charger >= 3) {
					{
						Projectile.NewProjectile(position.X, position.Y - 10, speedX, speedY, mod.ProjectileType("Stars3"), 130, 6, player.whoAmI, 0f, 0f);
					}
					charger = 0;
				}
				return false;
			}
			else {
				charger = 0;
				for (int I = 0; I < 2; I++) {
					Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-102, 102) / 100), speedY + ((float)Main.rand.Next(-102, 102) / 100), ModContent.ProjectileType<StarPin>(), damage, knockBack, player.whoAmI, 0f, 0f);
				};
			}
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				item.useTime = 37;
				item.useAnimation = 37;
				MyPlayer modPlayer = player.GetSpiritPlayer();
				if (modPlayer.shootDelay == 0)
					return true;
				return false;
			}
			else {
				item.useTime = 23;
				item.useAnimation = 23;
				return true;
			}
		}
	}
}