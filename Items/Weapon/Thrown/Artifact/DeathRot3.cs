using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Thrown.Artifact;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown.Artifact
{
	public class DeathRot3 : ModItem
	{
		int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Rot");
			Tooltip.SetDefault("Hit enemies are afflicted by 'Necrosis,'\nEvery fifth throw of the weapon leaves behind multiple clouds of Plague Miasma\nAttacks may release a swarm of Rot Seekers that explode into violent fumes\nCritical hits may cause enemies to explode into violent fumes");

		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 48;
			item.height = 52;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item100;
			item.melee = true;
			item.channel = true;
			item.crit = 6;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("DeathRot3Proj");
			item.useAnimation = 14;
			item.consumable = true;
			item.useTime = 14;
			item.shootSpeed = 11f;
			item.damage = 52;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 7, 0, 50);
			item.rare = ItemRarityID.Lime;
			item.autoReuse = true;
			item.maxStack = 1;
			item.consumable = false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			charger++;
			if (charger >= 5) {
				for (int I = 0; I < 3; I++) {
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<Miasma>(), damage, knockBack, player.whoAmI, 0f, 0f);
				}
				charger = 0;
			}
			if (Main.rand.Next(4) == 1) {
				for (int I = 0; I < 3; I++) {
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-130, 130) / 100), speedY + ((float)Main.rand.Next(-130, 130) / 100), ModContent.ProjectileType<RotSeeker>(), 40, knockBack, player.whoAmI, 0f, 0f);

				}
			}
			return true;
		}
	}
}
