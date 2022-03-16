using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloodcourtSet.BloodCourt
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodCourtHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Visage");
			Tooltip.SetDefault("4% increased damage\nIncreases your max number of minions");

		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = ItemRarityID.Green;
			item.defense = 3;
		}
        public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.04f;
			player.maxMinions += 1;
		}

		public override void UpdateVanity(Player player, EquipType type) => player.GetSpiritPlayer().bloodCourtHead = true;

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = true;

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.GetSpiritPlayer().bloodcourtSet = true;
			player.setBonus = $"Double tap {tapDir} to sacrifice 8% of your maximum health\n" +
							   "and launch a bolt of Dark Anima dealing high damage in a radius\n" +
							   "This bolt siphons 10 additional health over 5 seconds";
		}

		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BloodCourtChestplate>() && legs.type == ModContent.ItemType<BloodCourtLeggings>();

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public static void DoubleTapEffect(Player player)
		{
			player.AddBuff(ModContent.BuffType<CourtCooldown>(), 500);
			Vector2 mouse = Main.MouseScreen + Main.screenPosition;
			Vector2 dir = Vector2.Normalize(mouse - player.Center) * 12;
			player.statLife -= (int)(player.statLifeMax * .08f);

			for (int i = 0; i < 18; i++)
			{
				int num = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<NightmareDust>(), 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .85f;
				if (Main.dust[num].position != player.Center)
					Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
			}

			Main.PlaySound(new LegacySoundStyle(2, 109));

			Projectile.NewProjectile(player.Center, dir, ModContent.ProjectileType<DarkAnima>(), 70, 0, player.whoAmI);
		}
	}
}
