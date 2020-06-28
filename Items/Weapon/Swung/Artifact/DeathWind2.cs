using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Artifact;
using SpiritMod.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
	public class DeathWind2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Wind");
			Tooltip.SetDefault("Summons a returning scythe on swing\nEach scythe takes up one minion slot\nAttacks inflict 'Death Wreathe'\nRight-click to cause nearby enemies to take far more damage\n6 second cooldown\nAttacks may ignore enemy defense\nAttacks may grant you 'Soul Reap,' greatly boosting life regeneration");

		}


		public override void SetDefaults()
		{
			item.damage = 39;
			item.summon = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 7, 0, 50);
			item.shoot = mod.ProjectileType("DeathWind2Proj");
			item.rare = 4;
			item.shootSpeed = 15f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
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
			if(player.altFunctionUse == 2) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
				Main.dust[dust].noGravity = true;
			} else {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if(Main.rand.Next(6) == 2)
				target.AddBuff(ModContent.BuffType<DeathWreathe>(), 240);
			if(Main.rand.Next(6) == 1)
				damage = damage + (int)(target.defense);
			if(Main.rand.Next(6) == 2)
				player.AddBuff(ModContent.BuffType<SoulReap>(), 240);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(player.altFunctionUse == 2) {

				MyPlayer modPlayer = player.GetSpiritPlayer();
				modPlayer.shootDelay1 = 360;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<SoulNet>(), 0, 0, player.whoAmI);
				return true;
			}
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2) {

				MyPlayer modPlayer = player.GetSpiritPlayer();
				if(modPlayer.shootDelay1 == 0)
					return true;
				return false;
			}
			return true;
		}
	}
}
