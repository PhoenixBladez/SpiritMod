using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Artifact;
using SpiritMod.Projectiles.Sword.Artifact;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
	public class Thanos1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard of Thanos");
			Tooltip.SetDefault("'As old as the dawn of Man'\nShoots out an afterimage of the Shard\nRight-click to summon a storm of rotating crystals around the player");

		}


		public override void SetDefaults()
		{
			item.damage = 22;
			item.melee = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 3, 0, 50);
			item.shoot = mod.ProjectileType("Thanos1Proj");
			item.rare = ItemRarityID.Green;
			item.shootSpeed = 9f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
		}

		public override void HoldItem(Player player)
		{
			if (player.GetSpiritPlayer().Resolve) {
				player.AddBuff(ModContent.BuffType<Resolve>(), 2);
			}
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

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Crystal>());
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2) {
				float kb = knockBack * .2f;
				Projectile.NewProjectile(player.Center.X - 100, player.Center.Y, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 100, player.Center.Y, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 115, player.Center.Y - 115, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X - 115, player.Center.Y + 115, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y + 110, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y - 110, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
				return false;
			}
			return true;
		}
	}
}