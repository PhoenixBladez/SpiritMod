using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Artifact;
using SpiritMod.Projectiles.Magic.Artifact;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.Artifact
{
	public class Solus2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solus");
			Tooltip.SetDefault("Shoots out three Bolts with varied effects\nPhoenix Bolts explode upon hitting foes and inflict 'Blaze'\nFrost Bolts may freeze enemies in place\nShadow Bolts pierce multiple enemies and inflict 'Shadow Burn,' which hinders enemy damage");
		}


		public override void SetDefaults()
		{
			item.damage = 28;
			item.magic = true;
			item.mana = 11;
			item.width = 46;
			item.height = 54;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 5, 0, 50);
			item.rare = ItemRarityID.LightRed;
			item.crit = 3;
			item.UseSound = SoundID.Item74;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<PhoenixBolt>();
			item.shootSpeed = 1f;
		}
		public override void HoldItem(Player player)
		{
			if (player.GetSpiritPlayer().HolyGrail) {
				player.AddBuff(ModContent.BuffType<Righteous>(), 2);

			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			{
				Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<PhoenixBolt>(), damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<DarkBolt>(), damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<FreezeBolt>(), damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
